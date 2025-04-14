using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Refit;

namespace Services.ExternalApi;

public interface ITaskApi : IApi
{
    [Get("/projects/{projectId}/tasks")]
    Task<IApiResponse<IEnumerable<TaskDto>>> GetAllAsync(
        Guid projectId,
        [Query] string? searchTerm);

    [Get("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse<TaskDto>> GetAsync(Guid projectId, Guid taskId);

    [Post("/projects/{projectId}/tasks")]
    Task<IApiResponse<TaskDto>> CreateAsync([Body] TaskModel model, Guid projectId);

    [Put("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse> UpdateAsync(Guid projectId, Guid taskId, [Body] JsonPatchDocument<TaskModel> model);

    [Patch("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse> PartialUpdateAsync(Guid projectId, Guid taskId, [Body] object model);

    [Patch("/projects/{projectId}/tasks/{taskId}/state")]
    Task<IApiResponse> UpdateStateAsync(Guid projectId, Guid taskId, UpdateTaskStateModel model);

    [Patch("/projects/{projectId}/tasks/{taskId}/order")]
    Task<IApiResponse> ReorderAsync(Guid projectId, Guid taskId, [Body] ReorderTasksModel model);

    [Delete("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse> DeleteAsync(Guid projectId, Guid taskId);
}
