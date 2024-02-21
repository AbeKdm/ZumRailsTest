using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ZumRailsPosts.Common
{
    public class MemoryCacheService
    {
        private readonly MemoryCache _cache;
        private readonly DateTimeOffset _defaultExpirationTime;

        public MemoryCacheService()
        {
            _cache = new MemoryCache("RepoCache");
            _defaultExpirationTime = DateTimeOffset.UtcNow.AddMinutes(60); // Default expiration time set to 60 minutes
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public void Set<T>(string key, T value, DateTimeOffset? expirationTime)
        {
            _cache.Set(key, value, expirationTime ?? _defaultExpirationTime);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
