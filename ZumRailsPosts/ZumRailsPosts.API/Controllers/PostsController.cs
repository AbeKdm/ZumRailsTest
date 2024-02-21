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
        
        //https://localhost:7047/api/Posts?tags=aa%2Caa&sortBy=id&direction=desc

        [HttpGet]
        [ProducesResponseType(typeof(PostsResponseResults), StatusCodes.Status200OK)] 
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

            var returnObject = new PostsResponseResults() { Posts = posts };
            return Ok(returnObject);
        }
    }
}
