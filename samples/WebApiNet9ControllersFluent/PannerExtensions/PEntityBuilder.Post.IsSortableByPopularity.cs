namespace Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.PannerExtensions
{
    using global::Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.EFModel;
    using global::Panner.Builders;

    public static partial class PEntityBuilderExtensions
    {
        /// <summary>Marks the entity as sortable by popularity.</summary>
        public static PEntityBuilder<Post> IsSortableByPopularity(this PEntityBuilder<Post> builder)
        {
            builder.GetOrCreateGenerator<ISortParticle<Post>, SortPostsByPopularityParticleGenerator>();
            return builder;
        }
    }
}
