using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panner;
using Panner.AspNetCore;
using Panner.AspNetCore.Samples.WebApiNet8MinimalFluent.EFModel;
using Panner.AspNetCore.Samples.WebApiNet8MinimalFluent.PannerExtensions;
using Views = Panner.AspNetCore.Samples.WebApiNet8MinimalFluent.Views;
using Panner.Builders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UsePanner(c =>
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

builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseInMemoryDatabase("BlogDb");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/posts", async (
    [FromServices] BlogContext blogContext,
    SortParticles<Post> sorts,
    FilterParticles<Post> filters) =>
{
    blogContext.Database.EnsureCreated();
    var result = await blogContext.Posts
        .Apply(filters)
        .Apply(sorts)
        .Select(x => new Views.Post
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            Creation = x.CreatedOn
        })
        .ToArrayAsync();
    return Results.Ok(result);
}).WithOpenApi();

app.Run();
