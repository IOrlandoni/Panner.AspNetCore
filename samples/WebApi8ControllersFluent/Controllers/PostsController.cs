namespace Panner.AspNetCore.Samples.WebApi8ControllersFluent.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Panner.AspNetCore.Samples.WebApi8ControllersFluent.EFModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;

        public PostsController(ILogger<PostsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts(
            [FromServices] BlogContext blogContext,
            [FromQuery] IReadOnlyCollection<ISortParticle<Post>> sorts,
            [FromQuery] IReadOnlyCollection<IFilterParticle<Post>> filters
        )
        {
            // Workaround for: https://github.com/aspnet/EntityFrameworkCore/issues/11666
            // This is only because we're seeding data OnModelCreation for an InMemoryDatabase
            blogContext.Database.EnsureCreated();

            var result = await blogContext.Posts
                    .Apply(filters)
                    .Apply(sorts)
                    .Select(x => new Views.Post()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Content = x.Content,
                        Creation = x.CreatedOn
                    })
                    .ToArrayAsync();

            return Ok(result);
        }
    }
}
