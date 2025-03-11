using Domain.Dtos;
using Domain.Dtos.Authorization;
using Domain.Models.Identity;
using Refit;

namespace Services.ExternalApi;

public interface IIdentityApi : IApi
{
    [Post("/register")]
    Task<IApiResponse<RegisterDto>> RegisterAsync(RegisterModel model);

    [Post("/login")]
    Task<IApiResponse<TokenDto>> LoginAsync(LoginModel model);

    [Post("/reset-password")]
    Task<IApiResponse> ResetPasswordAsync(ResetPasswordModel model);

    [Post("/set-password")]
    Task<IApiResponse<TokenDto>> SetPasswordAsync(SetPasswordModel model);

    [Post("/change-password")]
    Task<IApiResponse> ChangePasswordAsync(ChangePasswordModel model);
}