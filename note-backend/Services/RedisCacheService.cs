using StackExchange.Redis;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace note_backend.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? exprice = null)
        {
            var jsonvalue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, jsonvalue, exprice);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var jsonData = await _database.StringGetAsync(key);
            return jsonData.HasValue ? JsonSerializer.Deserialize<T>(jsonData) : default;
        }

        public Task<bool> DelAsync(string key)
        {
            return _database.KeyDeleteAsync(key);
        }
    }
}
