using Domain.Models.Identity;
using Services.ExternalApi;
using Services.Interfaces;

namespace Services.Services.Components;

public sealed class IdentityService : IIdentityService
{
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly IIdentityApi _identityApi;

    public IdentityService(
        CustomAuthStateProvider authStateProvider, IIdentityApi identityApi)
    {
        _authStateProvider = authStateProvider;
        _identityApi = identityApi;
    }

    public async Task LoginAsync(LoginModel model)
    {
        var response = await _identityApi
            .LoginAsync(model);
        
        var tokenResponse = response.Content;

        _authStateProvider
            .MarkUserAsAuthenticated(tokenResponse.Token);
    }

    public async Task RegisterAsync(RegisterModel model)
    {
        var response = await _identityApi.RegisterAsync(model);

        var tokenResponse = response.Content.Token;

        _authStateProvider
            .MarkUserAsAuthenticated(tokenResponse.Token);
    }

    public void Logout()
    {
        _authStateProvider
            .MarkUserAsLoggedOut();
    }
}
