using Domain.Abstract;
using Domain.Dtos;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class IdentityService : IIdentityService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IIdentityApi _identityApi;

    public IdentityService(
        AuthenticationStateProvider authStateProvider,
        IIdentityApi identityApi)
    {
        _authStateProvider = authStateProvider;
        _identityApi = identityApi;
    }

    public async Task<Result<TokenDto>> LoginAsync(LoginModel model)
    {
        var response = await _identityApi
            .LoginAsync(model);

        return response.HandleResponse();
    }

    public async Task<Result<RegisterDto>> RegisterAsync(RegisterModel model)
    {
        var response = await _identityApi
            .RegisterAsync(model);

        return response.HandleResponse();
    }

    public async Task<Result> ResetPassword(ResetPasswordModel model)
    {
        var response = await _identityApi
            .ResetPasswordAsync(model);

        return response.HandleResponse();
    }

    public async Task<Result<TokenDto>> SetPasswordAndAuthorize(SetPasswordModel model)
    {
        var response = await _identityApi
            .SetPasswordAsync(model);

        return response.HandleResponse();
    }

    public async Task<Result> ChangePassword(ChangePasswordModel model)
    {
        var response = await _identityApi
            .ChangePasswordAsync(model);

        return response.HandleResponse();
    }
}
