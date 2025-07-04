using Panner.AspNetCore.Samples.WebApiNet8MinimalFluent.EFModel;

namespace Panner.AspNetCore.Samples.WebApiNet8MinimalFluent.PannerExtensions
{
    public class SortPostsByPopularityParticleGenerator : ISortParticleGenerator<Post>
    {
        public bool TryGenerate(IPContext context, string input, out ISortParticle<Post> particle)
        {
            var descending = input.StartsWith('-');
            var remaining = descending ? input.Substring(1) : input;

            if (!remaining.Trim().Equals("Popularity", System.StringComparison.OrdinalIgnoreCase))
            {
                particle = null;
                return false;
            }

            particle = new SortPostByPopularityParticle(descending);
            return true;
        }
    }
}
