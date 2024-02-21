using Microsoft.Extensions.Hosting;

namespace ZumRailsPosts.API.Models.DTO
{
    public class PostsResponseResults
    {
        public PostsResponseResults() { }
        public PostsResponseResults(List<Post> posts)
        {
            Posts = posts;
        }
        public List<Post> Posts { get; set; }
    }
}
