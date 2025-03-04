using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface ITaskApi : IApi
{
    [Get("/projects/{projectId}/tasks")]
    Task<IApiResponse<IEnumerable<TaskDto>>> GetTasksAsync(Guid projectId);

    [Get("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse<TaskDto>> GetTaskAsync(Guid projectId, Guid taskId);

    [Post("/projects/{projectId}/tasks")]
    Task<IApiResponse<TaskDto>> CreateTaskAsync(TaskModel model, Guid projectId);

    [Put("/projects/{projectId}/tasks/{model.Id}")]
    Task<IApiResponse> UpdateTaskAsync(TaskModel model, Guid projectId);

    [Delete("/projects/{projectId}/tasks/{taskId}")]
    Task<IApiResponse> DeleteTaskAsync(Guid projectId, Guid taskId);
}
