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

    public async Task<Result<TaskDto>> CreateAsync(
        TaskModel model,
        Guid projectId)
    {
        var response = await _taskApi
            .CreateAsync(projectId, model);

        return response
            .HandleResponse();
    }

    public async Task<Result> DeleteAsync(
        Guid projectId,
        Guid taskId)
    {
        var response = await _taskApi
            .DeleteAsync(projectId, taskId);

        return response
            .HandleResponse();
    }

    public async Task<Result<TaskDto>> GetAsync(
        Guid projectId,
        Guid taskId)
    {
        var response = await _taskApi
            .GetAsync(projectId, taskId);

        return response
            .HandleResponse();
    }

    public async Task<Result<IEnumerable<TaskDto>>> GetAllAsync(
        Guid projectId,
        string? searchTerm = null)
    {
        var response = await _taskApi
            .GetAllAsync(projectId, searchTerm);

        return response
            .HandleResponse();
    }

    public async Task<Result> UpdateAsync(
        Guid projectId,
        Guid taskId,
        TaskModel model)
    {
        var response = await _taskApi
            .UpdateAsync(projectId, taskId, model);

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

    public async Task<Result> AddTagAsync(Guid projectId, Guid taskId, Guid tagId)
    {
        var response = await _taskApi
            .AddTagAsync(projectId, taskId, tagId);

        return response
            .HandleResponse();
    }

    public async Task<Result> RemoveTagAsync(Guid projectId, Guid taskId, Guid tagId)
    {
        var response = await _taskApi
            .RemoveTagAsync(projectId, taskId, tagId);

        return response
            .HandleResponse();
    }
}
