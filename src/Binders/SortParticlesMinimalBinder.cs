using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Panner.AspNetCore
{
    public sealed class SortParticlesMinimalBinder<TEntity> : IReadOnlyCollection<ISortParticle<TEntity>>
        where TEntity : class
    {
        private readonly IReadOnlyCollection<ISortParticle<TEntity>> _particles;

        private SortParticlesMinimalBinder(IReadOnlyCollection<ISortParticle<TEntity>> particles)
        {
            _particles = particles;
        }

        public static ValueTask<SortParticlesMinimalBinder<TEntity>> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var queryName = parameter.Name ?? "sorts";
            StringValues value = context.Request.Query[queryName];

            var pContext = context.RequestServices.GetRequiredService<IPContext>();

            if (StringValues.IsNullOrEmpty(value))
            {
                return ValueTask.FromResult(new SortParticlesMinimalBinder<TEntity>(Array.Empty<ISortParticle<TEntity>>()));
            }

            if (!pContext.TryParseCsv(value.ToString(), out IEnumerable<ISortParticle<TEntity>> particles))
            {
                throw new InvalidOperationException("Could not parse provided sorts.");
            }

            return ValueTask.FromResult(new SortParticlesMinimalBinder<TEntity>(particles.ToArray()));
        }

        public int Count => _particles.Count;
        public IEnumerator<ISortParticle<TEntity>> GetEnumerator() => _particles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IReadOnlyCollection<ISortParticle<TEntity>> Value => _particles;
    }
}
