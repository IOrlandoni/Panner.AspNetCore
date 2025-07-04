using Microsoft.EntityFrameworkCore;
using Panner.AspNetCore;
using Panner.AspNetCore.Samples.FastEndpointsNet9;
using Panner.AspNetCore.Samples.FastEndpointsNet9.EFModel;
using Panner.AspNetCore.Samples.FastEndpointsNet9.PannerExtensions;
using Views = Panner.AspNetCore.Samples.FastEndpointsNet9.Views;
using Panner.Builders;

var bld = WebApplication.CreateBuilder(args);

bld.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["Auth:JwtKey"])
    .AddAuthorization()
    .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All)
    .SwaggerDocument();

bld.Services.UsePanner(c =>
{
    c.Entity<Post>()
        .IsSortableByPopularity()
        .Property(x => x.Id, o => o
            .IsSortableAs(nameof(Views.Post.Id))
            .IsFilterableAs(nameof(Views.Post.Id))
        )
        .Property(x => x.Title, o => o
            .IsSortableAs(nameof(Views.Post.Title))
        )
        .Property(x => x.CreatedOn, o => o
            .IsSortableAs(nameof(Views.Post.Creation))
            .IsFilterableAs(nameof(Views.Post.Creation))
        );
});

bld.Services.AddDbContext<BlogContext>(o => o.UseInMemoryDatabase("BlogDb"));

var app = bld.Build();
app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c =>
    {
        c.Errors.UseProblemDetails();
    })
    .UseSwaggerGen();
app.Run();
