using Microsoft.AspNetCore.Mvc;

using Panner;
using Panner.AspNetCore.Samples.FastEndpointsNet9.EFModel;
using FastEndpoints;

namespace Posts;

sealed class Request
{
    [FastEndpoints.FromQuery]
    public IReadOnlyCollection<ISortParticle<Post>>? Sorts { get; set; }

    [FastEndpoints.FromQuery]
    public IReadOnlyCollection<IFilterParticle<Post>>? Filters { get; set; }
}
