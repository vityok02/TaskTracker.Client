using Domain.Abstract;
using Domain.Models.Identity;

namespace Services.Interfaces;

public interface IIdentityService
{
    Task LoginAsync(LoginModel model);

    Task RegisterAsync(RegisterModel model);

    void Logout();
}
