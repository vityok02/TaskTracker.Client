using Domain.Dtos;
using Domain.Dtos.Authorization;
using Domain.Models.Identity;
using Refit;

namespace Services.ExternalApi;

public interface IIdentityApi : IApi
{
    [Post("/users/register")]
    Task<IApiResponse<RegisterDto>> RegisterAsync(RegisterModel model);

    [Post("/users/login")]
    Task<IApiResponse<TokenDto>> LoginAsync(LoginModel model);

    [Post("/users/reset-password")]
    Task<IApiResponse> ResetPasswordAsync(ResetPasswordModel model);

    [Post("/users/set-password")]
    Task<IApiResponse<TokenDto>> SetPasswordAsync(SetPasswordModel model);

    [Post("/users/change-password")]
    Task<IApiResponse> ChangePasswordAsync(ChangePasswordModel model);
}