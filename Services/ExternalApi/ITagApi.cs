using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface ITagApi : IApi
{
    [Post("/projects/{projectId}/tags")]
    Task<IApiResponse<TagDto>> CreateAsync(Guid projectId, [Body] TagModel model);

    [Get("/projects/{projectId}/tags/{tagId}")]
    Task<IApiResponse<TagDto>> GetAsync(Guid projectId, Guid tagId);

    [Get("/projects/{projectId}/tags")]
    Task<IApiResponse<IEnumerable<TagDto>>> GetAllAsync(
        Guid projectId);

    [Put("/projects/{projectId}/tags/{tagId}")]
    Task<IApiResponse<TagDto>> UpdateAsync(Guid projectId, Guid tagId, [Body] TagModel model);

    [Delete("/projects/{projectId}/tags/{tagId}")]
    Task<IApiResponse> DeleteAsync(Guid projectId, Guid tagId);
}
