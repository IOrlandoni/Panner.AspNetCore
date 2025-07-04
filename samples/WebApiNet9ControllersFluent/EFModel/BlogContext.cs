namespace Panner.AspNetCore.Samples.WebApiNet9ControllersFluent.EFModel
{
    using Microsoft.EntityFrameworkCore;
    using System;

    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogContext() : base()
        {
        }

        public BlogContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasData(new Post()
                {
                    Id = 1,
                    Title = Guid.NewGuid().ToString(),
                    Content = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.UtcNow,
                    IsVisible = true
                }, new Post()
                {
                    Id = 2,
                    Title = Guid.NewGuid().ToString(),
                    Content = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.UtcNow,
                    IsVisible = false
                }, new Post()
                {
                    Id = 3,
                    Title = Guid.NewGuid().ToString(),
                    Content = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.UtcNow,
                    IsVisible = true
                });
        }
    }
}
