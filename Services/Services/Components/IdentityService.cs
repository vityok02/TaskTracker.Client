using Domain.Abstract;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Services.ExternalApi;
using Services.Interfaces;

namespace Services.Services.Components;

public class IdentityService : IIdentityService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IIdentityApi _identityApi;

    public IdentityService(
        AuthenticationStateProvider authStateProvider, IIdentityApi identityApi)
    {
        _authStateProvider = authStateProvider;
        _identityApi = identityApi;
    }

    public async Task<Result> LoginAsync(LoginModel model)
    {
        var response = await _identityApi
            .LoginAsync(model);
        
        var tokenResponse = response.Content;

        (_authStateProvider as CustomAuthStateProvider)!
            .MarkUserAsAuthenticated(tokenResponse.Token);

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterModel model)
    {
        var response = await _identityApi.RegisterAsync(model);

        var tokenResponse = response.Content.Token;

        (_authStateProvider as CustomAuthStateProvider)!
            .MarkUserAsAuthenticated(tokenResponse.Token);

        return Result.Success();
    }

    public void Logout()
    {
        (_authStateProvider as CustomAuthStateProvider)!
            .MarkUserAsLoggedOut();
    }
}
