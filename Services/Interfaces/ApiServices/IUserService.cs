using Domain.Abstract;
using Domain.Dtos;

namespace Services.Interfaces.ApiServices;

public interface IUserService
{
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);

    Task<Result<IEnumerable<UserDto>>> GetUsersAsync();

    Task<Result<IEnumerable<UserDto>>> SearchUsersAsync(string Username);
}