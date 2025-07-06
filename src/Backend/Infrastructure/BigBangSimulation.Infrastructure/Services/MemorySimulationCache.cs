using BigBangSimulation.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BigBangSimulation.Infrastructure.Services
{
    public class MemorySimulationCache : ISimulationCache
    {
        private readonly IMemoryCache _memoryCache;

        public MemorySimulationCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key) where T : class
        {
            var cached = _memoryCache.Get(key);
            if (cached is string jsonString)
            {
                var result = JsonSerializer.Deserialize<T>(jsonString);
                return Task.FromResult(result);
            }
            return Task.FromResult<T?>(null);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }
            
            var jsonString = JsonSerializer.Serialize(value);
            _memoryCache.Set(key, jsonString, options);
            
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            var exists = _memoryCache.TryGetValue(key, out _);
            return Task.FromResult(exists);
        }
    }
}
