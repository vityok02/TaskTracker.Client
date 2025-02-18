using Domain.Abstract;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.Components;

namespace Services.Services.Components;

public class IdentityService : IIdentityService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IIdentityApi _identityApi;

    private readonly string _errorMessage
        = "Authentication failed. Please try again.";
    private readonly string _authProviderError
        = "Authentication failed. Please try again later.";

    public IdentityService(
        AuthenticationStateProvider authStateProvider,
        IIdentityApi identityApi)
    {
        _authStateProvider = authStateProvider;
        _identityApi = identityApi;
    }

    public async Task<Result> LoginAsync(LoginModel model)
    {
        var response = await _identityApi
            .LoginAsync(model);

        return await HandleAuthResponseAsync(
            response,
            response.Content?.Token);
    }

    public async Task<Result> RegisterAsync(RegisterModel model)
    {
        var response = await _identityApi
            .RegisterAsync(model);

        return await HandleAuthResponseAsync(
            response,
            response.Content?.Token?.Token);
    }

    public async Task Logout()
    {
        if (_authStateProvider is CustomAuthStateProvider provider)
        {
            await provider
                .MarkUserAsLoggedOutAsync();
        }
    }

    public async Task<Result> ResetPassword(ResetPasswordModel model)
    {
        var response = await _identityApi
            .ResetPasswordAsync(model);

        return response.HandleResponse();
    }

    public async Task<Result> SetPasswordAndAuthorize(SetPasswordModel model)
    {
        var response = await _identityApi
            .SetPasswordAsync(model);

        return await HandleAuthResponseAsync(
            response, response.Content?.Token);
    }

    public async Task<Result> ChangePassword(ChangePasswordModel model)
    {
        var response = await _identityApi
            .ChangePasswordAsync(model);

        return response.HandleResponse();
    }

    private async Task<Result> HandleAuthResponseAsync(
        IApiResponse response,
        string? token)
    {
        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = response
                .GetProblemDetails();

            var errorType = problemDetails.Type
                ?? "AuthenticationError";
            var errorDetail = problemDetails.Detail
                ?? _errorMessage;

            return Result.Failure(problemDetails.Errors is null
                ? new Error(errorType, errorDetail)
                : new ValidationError(errorType, errorDetail, problemDetails.Errors));
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return Result
                .Failure(new Error("InvalidToken", _errorMessage));
        }

        return await AuthenticateUserAsync(token);
    }

    private async Task<Result> AuthenticateUserAsync(string token)
    {
        if (_authStateProvider is not CustomAuthStateProvider provider)
        {
            return Result
                .Failure("AuthProviderError", _authProviderError);
        }

        await provider
            .MarkUserAsAuthenticatedAsync(token);

        return Result.Success();
    }
}
