using Domain.Abstract;
using Domain.Models.Identity;

namespace Services.Interfaces;

public interface IIdentityService
{
    Task<Result> LoginAsync(LoginModel model);

    Task<Result> RegisterAsync(RegisterModel model);

    Task Logout();
}
