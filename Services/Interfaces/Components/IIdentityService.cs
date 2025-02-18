using Domain.Abstract;
using Domain.Dtos;
using Domain.Models.Identity;

namespace Services.Interfaces.Components;

public interface IIdentityService
{
    Task<Result> LoginAsync(LoginModel model);

    Task<Result> RegisterAsync(RegisterModel model);

    Task Logout();

    Task<Result> ResetPassword(ResetPasswordModel model);

    Task<Result> SetPasswordAndAuthorize(SetPasswordModel model);

    Task<Result> ChangePassword(ChangePasswordModel model);
}
