using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class TaskService : ITaskService
{
    private readonly ITaskApi _taskApi;

    public TaskService(ITaskApi taskApi)
    {
        _taskApi = taskApi;
    }

    public async Task<Result<TaskDto>> CreateTaskAsync(TaskModel model, Guid projectId)
    {
        var response = await _taskApi
            .CreateTaskAsync(model, projectId);

        return response
            .HandleResponse();
    }

    public async Task<Result> DeleteTaskAsync(Guid projectId, Guid taskId)
    {
        var response = await _taskApi
            .DeleteTaskAsync(projectId, taskId);

        return response
            .HandleResponse();
    }

    public async Task<Result<TaskDto>> GetTaskAsync(Guid projectId, Guid taskId)
    {
        var response = await _taskApi
            .GetTaskAsync(projectId, taskId);

        return response
            .HandleResponse();
    }

    public async Task<Result<IEnumerable<TaskDto>>> GetTasksAsync(Guid projectId)
    {
        var response = await _taskApi
            .GetTasksAsync(projectId);

        return response
            .HandleResponse();
    }

    public async Task<Result> UpdateTaskAsync(TaskModel model, Guid projectId)
    {
        var response = await _taskApi
            .UpdateTaskAsync(model, projectId);

        return response
            .HandleResponse();
    }
}
