using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZumRailsPosts.Core.Infrastructure;
using ZumRailsPosts.Core.Infrastructure.Models;

namespace ZumRailsPosts.Core.Logic
{
    /// <summary>
    /// Provides business logic related to posts.
    /// </summary>
    public class PostLogic : IPostsLogic
    {
        private readonly IPostsRepository _repo;

        public PostLogic(IPostsRepository repo)
        {
            _repo = repo;
        }


        /// <summary>
        /// Retrieves posts asynchronously based on specified tags, with optional sorting and direction.
        /// </summary>
        /// <param name="tags">Tags to filter posts by.</param>
        /// <param name="sortBy">Field to sort the posts by (default is 'id').</param>
        /// <param name="direction">Sorting direction, either 'desc' (default) or 'asc'.</param>
        /// <returns>Returns a list of posts based on the specified criteria.</returns>
        public async Task<List<Post>> GetPostsAsync(string[] tags, string sortBy = "id", string direction = "desc")
        {
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
