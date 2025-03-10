using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface ITaskService
{
    Task<Result<IEnumerable<TaskDto>>> GetTasksAsync(Guid projectId);

    Task<Result<TaskDto>> GetTaskAsync(Guid projectId, Guid taskId);

    Task<Result<TaskDto>> CreateTaskAsync(TaskModel model, Guid projectId);

    Task<Result> UpdateTaskAsync(TaskModel model, Guid projectId);

    Task<Result> DeleteTaskAsync(Guid projectId, Guid taskId);
}
