using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ZumRailsPosts.API.Attributes.Validations;
using ZumRailsPosts.API.Errors;
using ZumRailsPosts.API.Models.DTO;
using ZumRailsPosts.Core.Logic;

namespace ZumRailsPosts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostsLogic _logic;

        public PostsController(ILogger<PostsController> logger, IPostsLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        /// <summary>
        /// Retrieves posts based on specified tags, with optional sorting and direction.
        /// </summary>
        /// <param name="tags">Tags to filter posts by. Multiple tags should be comma-separated.</param>
        /// <param name="sortBy">Field to sort the posts by (default is 'id').</param>
        /// <param name="direction">Sorting direction, either 'asc' (default) or 'desc'.</param>
        /// <returns>Returns a collection of posts based on the specified criteria.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PostsResponseResults), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PostsResponseResults>> GetPosts(
                    [Required(ErrorMessage = "The 'tags' parameter is required.")]
                    [FromQuery(Name = "tags")] string tags,
                    [PostsSort]
                    [FromQuery(Name = "sortBy")] string sortBy = "id",
                    [Direction]
                    [FromQuery(Name = "direction")] string direction = "asc")
        {
            // Validation took care invalid model state, reffer to Program.cs

            var posts = await _logic.GetPostsAsync(tags.Split(","), sortBy, direction);

            if (posts == null || posts.Count == 0) 
            { 
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No posts were found matching your criteria."));
            }

            var returnObject = new PostsResponseResults(posts);

            return Ok(returnObject);
        }
    }
}
