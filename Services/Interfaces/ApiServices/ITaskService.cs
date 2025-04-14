using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Services.Interfaces.ApiServices;

public interface ITaskService
{
    Task<Result<IEnumerable<TaskDto>>> GetAllAsync(Guid projectId, string? searchTerm);

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

    Task<Result> PartialUpdateAsync(
        Guid projectId,
        Guid taskId,
        TaskModel model);

    Task<Result> DeleteAsync(Guid projectId, Guid taskId);
}
