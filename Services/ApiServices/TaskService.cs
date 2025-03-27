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

    public async Task<Result<TaskDto>> CreateAsync(TaskModel model, Guid projectId)
    {
        var response = await _taskApi
            .CreateAsync(model, projectId);

        return response
            .HandleResponse();
    }

    public async Task<Result> DeleteAsync(Guid projectId, Guid taskId)
    {
        var response = await _taskApi
            .DeleteAsync(projectId, taskId);

        return response
            .HandleResponse();
    }

    public async Task<Result<TaskDto>> GetAsync(Guid projectId, Guid taskId)
    {
        var response = await _taskApi
            .GetAsync(projectId, taskId);

        return response
            .HandleResponse();
    }

    public async Task<Result<IEnumerable<TaskDto>>> GetAllAsync(Guid projectId)
    {
        var response = await _taskApi
            .GetAllAsync(projectId);

        return response
            .HandleResponse();
    }

    public async Task<Result> UpdateAsync(TaskModel model, Guid projectId)
    {
        var response = await _taskApi
            .UpdateAsync(model, projectId);

        return response
            .HandleResponse();
    }

    public async Task<Result> UpdateStateAsync(
        Guid projectId,
        Guid taskId,
        UpdateTaskStateModel model)
    {
        var response = await _taskApi
            .UpdateStateAsync(projectId, taskId, model);

        return response
            .HandleResponse();
    }

    public async Task<Result> ReorderAsync(
        Guid projectId,
        Guid taskId,
        ReorderTasksModel model)
    {
        var response = await _taskApi
            .ReorderAsync(projectId, taskId, model);

        return response
            .HandleResponse();
    }
}
