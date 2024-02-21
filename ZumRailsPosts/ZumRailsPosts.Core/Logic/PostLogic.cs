using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZumRailsPosts.Common.Entities;
using ZumRailsPosts.Core.Infrastructure;

namespace ZumRailsPosts.Core.Logic
{
    public class PostLogic : IPostsLogic
    {
        private readonly IPostsRepository _repo;

        public PostLogic(IPostsRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<Post>> GetPostsAsync(string[] tags, string sortBy = "id", string direction = "desc")
        {
            /*
             * TODO :
             * for each tag
             *  Create multiple calls to get posts by single tag
             * 
             * Union (\set) - remove dups
             * 
             * sort 
             * direction
             */

            List<Post> posts = new List<Post>();

            var distinctTags = tags.Distinct(StringComparer.CurrentCultureIgnoreCase); // IEnumerable

            foreach (var tag in distinctTags)
            {
                // option: to implement a retry machanism 
                posts.AddRange(await _repo.GetAllPostsAsync(tag)); 
            }

            var distinctPosts = posts.Distinct();

            var sortedPosts = SortPosts(distinctPosts, sortBy, direction);

            return sortedPosts;
        }

        private List<Post> SortPosts(IEnumerable<Post> posts, string sortBy, string direction)
        {
            if (posts == null || !posts.Any())
                return new List<Post>();

            var sortExpression = GetSortExpression(sortBy);

            if (direction.ToLower() == "desc")
            {
                return posts.OrderByDescending(sortExpression.Compile()).ToList();
            }
            else if (direction.ToLower() == "asc")
            {
                return posts.OrderBy(sortExpression.Compile()).ToList();
            }
            else
            {
                throw new ArgumentException($"Invalid direction parameter: {direction}");
            }
        }

        private Expression<Func<Post, object>> GetSortExpression(string sortBy)
        {
            Expression<Func<Post, object>> sortExpression;
            switch (sortBy.ToLower())
            {
                case "id":
                    sortExpression = post => post.Id;
                    break;
                case "reads":
                    sortExpression = post => post.Reads;
                    break;
                case "likes":
                    sortExpression = post => post.Likes;
                    break;
                case "popularity":
                    sortExpression = post => post.Popularity;
                    break;
                default:
                    throw new ArgumentException($"Invalid sortBy parameter: {sortBy}");
            }
            return sortExpression;
        }

        //private List<Post> removeDupPosts(List<Post> posts) => new HashSet<Post>(posts).ToList();

        //private List<Post> removeDupPosts2(List<Post> posts) => posts.Distinct().ToList();
    }
}
