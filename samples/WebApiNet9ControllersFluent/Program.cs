using Microsoft.EntityFrameworkCore;
using Panner.AspNetCore;
using Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.EFModel;
using Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.PannerExtensions;
using Views = Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.Views;
using Panner.Builders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

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
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
