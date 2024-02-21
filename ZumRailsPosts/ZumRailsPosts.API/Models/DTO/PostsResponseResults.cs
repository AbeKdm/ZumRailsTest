using Microsoft.Extensions.Hosting;
using ZumRailsPosts.Common.Entities;

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
