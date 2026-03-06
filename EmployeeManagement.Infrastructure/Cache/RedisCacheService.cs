using StackExchange.Redis;
using System.Text.Json;

namespace EmployeeManagement.Infrastructure.Cache;

public class RedisCacheService
{
    private readonly IDatabase _cache;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task SetAsync(string key, object value)
    {
        var json = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(key, json);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }
}