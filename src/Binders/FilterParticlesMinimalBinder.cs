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
    public sealed class FilterParticlesMinimalBinder<TEntity> : IReadOnlyCollection<IFilterParticle<TEntity>>
        where TEntity : class
    {
        private readonly IReadOnlyCollection<IFilterParticle<TEntity>> _particles;

        private FilterParticlesMinimalBinder(IReadOnlyCollection<IFilterParticle<TEntity>> particles)
        {
            _particles = particles;
        }

        public static ValueTask<FilterParticlesMinimalBinder<TEntity>> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var queryName = parameter.Name ?? "filters";
            StringValues value = context.Request.Query[queryName];

            var pContext = context.RequestServices.GetRequiredService<IPContext>();

            if (StringValues.IsNullOrEmpty(value))
            {
                return ValueTask.FromResult(new FilterParticlesMinimalBinder<TEntity>(Array.Empty<IFilterParticle<TEntity>>()));
            }

            if (!pContext.TryParseCsv(value.ToString(), out IEnumerable<IFilterParticle<TEntity>> particles))
            {
                throw new InvalidOperationException("Could not parse provided filters.");
            }

            return ValueTask.FromResult(new FilterParticlesMinimalBinder<TEntity>(particles.ToArray()));
        }

        public int Count => _particles.Count;
        public IEnumerator<IFilterParticle<TEntity>> GetEnumerator() => _particles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IReadOnlyCollection<IFilterParticle<TEntity>> Value => _particles;
    }
}
