using Microsoft.AspNetCore.Mvc;
using Panner;
using Panner.AspNetCore.Samples.FastEndpointsNet9.EFModel;

namespace Posts;

sealed class Request
{
    [FromQuery]
    public IReadOnlyCollection<ISortParticle<Post>>? Sorts { get; set; }

    [FromQuery]
    public IReadOnlyCollection<IFilterParticle<Post>>? Filters { get; set; }
}
