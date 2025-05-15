using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface ITaskApi : IApi
{
    [Get("/projects/{projectId}/tasks")]
    Task<IApiResponse<IEnumerable<TaskDto>>> GetAllAsync(
        Guid projectId,
        [Query] string? searchTerm,
        [Query(CollectionFormat.Multi)] Guid[]? tagIds = null);

    [Get("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse<TaskDto>> GetAsync(Guid projectId, Guid taskId);

    [Post("/projects/{projectId}/tasks")]
    Task<IApiResponse<TaskDto>> CreateAsync(Guid projectId, [Body] TaskModel model);

    [Put("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse> UpdateAsync(Guid projectId, Guid taskId, [Body] TaskModel model);

    [Put("/projects/{projectId}/tasks/{taskId}/tags/{tagId}")]
    Task<IApiResponse> AddTagAsync(Guid projectId, Guid taskId, Guid tagId);

    [Delete("/projects/{projectId}/tasks/{taskId}/tags/{tagId}")]
    Task<IApiResponse> RemoveTagAsync(Guid projectId, Guid taskId, Guid tagId);

    [Patch("/projects/{projectId}/tasks/{taskId}/state")]
    Task<IApiResponse> UpdateStateAsync(Guid projectId, Guid taskId, [Body] UpdateTaskStateModel model);

    [Patch("/projects/{projectId}/tasks/{taskId}/order")]
    Task<IApiResponse> ReorderAsync(Guid projectId, Guid taskId, [Body] ReorderTasksModel model);

    [Delete("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse> DeleteAsync(Guid projectId, Guid taskId);
}
