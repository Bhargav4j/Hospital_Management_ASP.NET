using System.Collections.Concurrent;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Services;

public class InMemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly ILogger<InMemoryCacheService> _logger;

    public InMemoryCacheService(ILogger<InMemoryCacheService> logger)
    {
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key) where T : class
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt.HasValue && entry.ExpiresAt.Value < DateTimeOffset.UtcNow)
            {
                _cache.TryRemove(key, out _);
                _logger.LogDebug("In-memory cache expired for key {Key}", key);
                return Task.FromResult<T?>(null);
            }

            _logger.LogDebug("In-memory cache hit for key {Key}", key);
            return Task.FromResult(entry.Value as T);
        }

        _logger.LogDebug("In-memory cache miss for key {Key}", key);
        return Task.FromResult<T?>(null);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        var entry = new CacheEntry
        {
            Value = value,
            ExpiresAt = expiration.HasValue
                ? DateTimeOffset.UtcNow.Add(expiration.Value)
                : DateTimeOffset.UtcNow.AddMinutes(30)
        };

        _cache.AddOrUpdate(key, entry, (_, _) => entry);
        _logger.LogDebug("In-memory cached value for key {Key}", key);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.TryRemove(key, out _);
        _logger.LogDebug("In-memory removed cache entry for key {Key}", key);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        var keysToRemove = _cache.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var key in keysToRemove)
        {
            _cache.TryRemove(key, out _);
        }
        _logger.LogDebug("In-memory removed {Count} cache entries with prefix {Prefix}", keysToRemove.Count, prefix);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key)
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt.HasValue && entry.ExpiresAt.Value < DateTimeOffset.UtcNow)
            {
                _cache.TryRemove(key, out _);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    private class CacheEntry
    {
        public object? Value { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}
