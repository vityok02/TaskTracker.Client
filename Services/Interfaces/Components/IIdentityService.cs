using Domain.Abstract;
using Domain.Models.Identity;

namespace Services.Interfaces.Components;

public interface IIdentityService
{
    Task<Result> LoginAsync(LoginModel model);

    Task<Result> RegisterAsync(RegisterModel model);

    Task Logout();
}
