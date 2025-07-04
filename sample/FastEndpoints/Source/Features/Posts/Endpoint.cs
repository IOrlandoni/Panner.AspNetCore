using Microsoft.EntityFrameworkCore;
using Panner;
using Panner.AspNetCore.Samples.FastEndpointsNet9.EFModel;
using Views = Panner.AspNetCore.Samples.FastEndpointsNet9.Views;
using Panner.AspNetCore.Samples.FastEndpointsNet9.PannerExtensions;

namespace Posts;

sealed class Endpoint : Endpoint<Request, IEnumerable<Views.Post>>
{
    private readonly BlogContext _context;

    public Endpoint(BlogContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/posts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        _context.Database.EnsureCreated();
        var result = await _context.Posts
            .Apply(r.Filters ?? Array.Empty<IFilterParticle<Post>>())
            .Apply(r.Sorts ?? Array.Empty<ISortParticle<Post>>())
            .Select(x => new Views.Post
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Creation = x.CreatedOn
            })
            .ToArrayAsync(c);

        await SendAsync(result, cancellation: c);
    }
}
