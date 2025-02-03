using Domain.Models.Identity;
using Services.Interfaces;
using System.Net.Http.Json;

namespace Services;

public sealed class IdentityService
{
    private readonly IHttpClientFactory _clientFactory;

    public IdentityService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task LoginAsync(LoginModel model)
    {
        using var client = _clientFactory.CreateClient();

        var result = await client.PostAsJsonAsync("https://localhost:5001/users/login", model);
    }

    public async Task RegisterAsync(RegisterModel model)
    {
        using var client = _clientFactory.CreateClient();

        var result = await client.PostAsJsonAsync("https://localhost:5001/users/register", model);
    }
}
