namespace Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.PannerExtensions
{
    using global::Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.EFModel;
    using System.Linq;

    public class SortPostByPopularityParticle : ISortParticle<Post>
    {
        readonly bool Descending;

        public SortPostByPopularityParticle(bool descending)
        {
            this.Descending = descending;
        }

        public IOrderedQueryable<Post> ApplyTo(IOrderedQueryable<Post> source)
        {
            if (this.Descending)
                return source
                    .ThenByDescending(x => x.AmtLikes)
                    .ThenByDescending(x => x.AmtComments);
            else
                return source
                    .ThenBy(x => x.AmtLikes)
                    .ThenBy(x => x.AmtComments);
        }
    }
}
