using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface IProjectMemberApi : IApi
{
    [Post("/projects/{projectId}/members")]
    Task<IApiResponse> CreateAsync(Guid projectId, [Body] ProjectMemberModel model);

    [Get("/projects/{projectId}/members")]
    Task<IApiResponse<IEnumerable<ProjectMemberDto>>> GetAllAsync(Guid projectId);

    [Put("/projects/{projectId}/members{memberId}")]
    Task<IApiResponse> UpdateAsync(Guid projectId, Guid memberId, [Body] ProjectMemberModel model);

    [Delete("/projects/{projectId}/members/{memberId}")]
    Task<IApiResponse> DeleteAsync(Guid projectId, Guid memberId);
}
