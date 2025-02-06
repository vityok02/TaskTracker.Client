using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;

namespace Services.Services;

public class TokenStorage : ITokenStorage
{
    private readonly IMemoryCache _memoryCache;

    private readonly TimeSpan ExpirationTime = TimeSpan.FromHours(24);

    public TokenStorage(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void SetToken(string token)
    {
        _memoryCache
            .Set("jwtToken", token, new MemoryCacheEntryOptions()
            .SetSlidingExpiration(ExpirationTime));
    }

    public string GetToken()
    {
        return _memoryCache
            .Get<string>("jwtToken") ?? string.Empty;
    }

    public void RemoveToken()
    {
        _memoryCache.Remove("jwtToken");
    }
}
