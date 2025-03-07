using Domain.Dtos;
using Refit;

namespace Services.ExternalApi;

public interface IRoleApi : IApi
{
    [Get("/roles")]
    Task<IApiResponse<IEnumerable<RoleDto>>> GetAllAsync();
}
