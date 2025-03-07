using Domain.Abstract;
using Domain.Dtos;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class RoleService : IRoleService
{
    private readonly IRoleApi _roleApi;

    public RoleService(IRoleApi roleApi)
    {
        _roleApi = roleApi;
    }
    public async Task<Result<IEnumerable<RoleDto>>> GetRolesAsync()
    {
        var response = await _roleApi.GetAllAsync();

        return response.HandleResponse();
    }
}
