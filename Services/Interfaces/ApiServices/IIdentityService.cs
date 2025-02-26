using Domain.Abstract;
using Domain.Dtos;
using Domain.Models.Identity;

namespace Services.Interfaces.ApiServices;

public interface IIdentityService
{
    Task<Result<TokenDto>> LoginAsync(LoginModel model);

    Task<Result<RegisterDto>> RegisterAsync(RegisterModel model);

    Task<Result> ResetPassword(ResetPasswordModel model);

    Task<Result<TokenDto>> SetPasswordAndAuthorize(SetPasswordModel model);

    Task<Result> ChangePassword(ChangePasswordModel model);
}
