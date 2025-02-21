using Services.Interfaces;

namespace Services.Services;

public class TokenStorage : ITokenStorage
{
    private const string Key = "jwtToken";

    private readonly ICookieManager _cookieManager;

    public TokenStorage(ICookieManager cookieManager)
    {
        _cookieManager = cookieManager;
    }

    public async Task SetToken(string token)
    {
        _cookieManager.Set(Key, token);

        await Task.CompletedTask;
    }

    public async Task<string?> GetToken()
    {
        return await Task
            .FromResult(_cookieManager.Get(Key));

    }

    public async Task RemoveToken()
    {
        _cookieManager.Remove(Key);
        await Task.CompletedTask;
    }
}
