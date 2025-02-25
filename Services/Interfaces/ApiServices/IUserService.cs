using Domain.Abstract;
using Domain.Dtos;

namespace Services.Interfaces.ApiServices;

public interface IUserService
{
    Task<Result<UserDto>> GetUserAsync(Guid userId);

    Task<Result<IEnumerable<UserDto>>> GetUsersAsync();
}