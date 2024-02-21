using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using ZumRailsPosts.Common;
using ZumRailsPosts.Common.Models;
using ZumRailsPosts.Core.Infrastructure.Models;

namespace ZumRailsPosts.Core.Infrastructure
{
    public class PostsRepository : IPostsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly MemoryCacheService _cache;

        public PostsRepository(HttpClient httpClient, string apiBaseUrl, MemoryCacheService cache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = apiBaseUrl ?? "https://api.hatchways.io";
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        
        public async Task<List<Post>> GetAllPostsAsync(string tag)
        {
            var cacheKey = $"posts#{tag}";
            var posts = _cache.Get< List<Post>>(cacheKey);
            if (posts == null)
            {
                var url = BuildPostsUrl(tag);
                var response = await FetchData<ApiResponse>(url);
                posts = response.Posts;

                // Caching the fetched categories with a 30-minute expiration policy.
                if (posts.Any())
                    _cache.Set(cacheKey, posts, DateTime.UtcNow.AddMinutes(30));
            }
            return posts;
        }

        private string BuildPostsUrl(string tag) => $"{_apiBaseUrl}/assessment/blog/posts/?tag={Uri.EscapeDataString(tag)}";

        private async Task<T> FetchData<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching data from the API.", ex);
            }
        }
    }
}
