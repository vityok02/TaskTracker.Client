using Domain.Abstract;
using Domain.Dtos;

namespace Services.Interfaces.ApiServices;

public interface IRoleService
{
    Task<Result<IEnumerable<RoleDto>>> GetRolesAsync();
}
