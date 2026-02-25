using System.Text.Json;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ClinicManagement.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer? _connectionMultiplexer;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly string _instanceName;
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(30);

    public RedisCacheService(
        IDistributedCache cache,
        ILogger<RedisCacheService> logger,
        IConnectionMultiplexer? connectionMultiplexer = null,
        string? instanceName = null)
    {
        _cache = cache;
        _logger = logger;
        _connectionMultiplexer = connectionMultiplexer;
        _instanceName = instanceName ?? "ClinicMgmt:";
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        try
        {
            var cachedValue = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedValue))
            {
                _logger.LogDebug("Redis cache miss for key {Key}", key);
                return null;
            }

            _logger.LogDebug("Redis cache hit for key {Key}", key);
            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis GET failed for key {Key}, returning null", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
            };

            var serializedValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, options);
            _logger.LogDebug("Redis cached value for key {Key} with expiration {Expiration}", key, expiration ?? DefaultExpiration);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis SET failed for key {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            _logger.LogDebug("Redis removed cache entry for key {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis REMOVE failed for key {Key}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        if (_connectionMultiplexer == null)
        {
            _logger.LogWarning("RemoveByPrefix skipped: IConnectionMultiplexer not available. Prefix: {Prefix}", prefix);
            return;
        }

        try
        {
            var fullPrefix = $"{_instanceName}{prefix}*";
            var endpoints = _connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: fullPrefix).ToArray();

                if (keys.Length > 0)
                {
                    var db = _connectionMultiplexer.GetDatabase();
                    await db.KeyDeleteAsync(keys);
                    _logger.LogDebug("Redis removed {Count} cache entries with prefix {Prefix}", keys.Length, prefix);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis RemoveByPrefix failed for prefix {Prefix}", prefix);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);
            return !string.IsNullOrEmpty(value);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis EXISTS failed for key {Key}", key);
            return false;
        }
    }
}
