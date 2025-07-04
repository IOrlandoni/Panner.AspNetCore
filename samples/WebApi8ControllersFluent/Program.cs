using Microsoft.EntityFrameworkCore;
using Panner.AspNetCore.Samples.WebApi8ControllersFluent.EFModel;
using Panner.AspNetCore.Samples.WebApi8ControllersFluent.PannerExtensions;
using Panner.Builders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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
app.UseAuthorization();
app.MapControllers();

app.Run();
