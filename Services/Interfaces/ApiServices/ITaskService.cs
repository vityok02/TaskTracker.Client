using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface ITaskService
{
    Task<Result<IEnumerable<TaskDto>>> GetAllAsync(
        Guid projectId,
        string? searchTerm = null,
        Guid[]? tagIds = null);

    Task<Result<TaskDto>> GetAsync(Guid projectId, Guid taskId);

    Task<Result<TaskDto>> CreateAsync(TaskModel model, Guid projectId);

    Task<Result> ReorderAsync(
        Guid projectId,
        Guid taskId,
        ReorderTasksModel model);

    Task<Result> UpdateStateAsync(
        Guid projectId,
        Guid taskId,
        UpdateTaskStateModel model);

    Task<Result> UpdateAsync(
        Guid projectId,
        Guid taskId,
        TaskModel model);

    Task<Result> AddTagAsync(
        Guid projectId,
        Guid taskId,
        Guid tagId);

    Task<Result> RemoveTagAsync(
        Guid projectId,
        Guid taskId,
        Guid tagId);

    Task<Result> DeleteAsync(Guid projectId, Guid taskId);
}
