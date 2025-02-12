using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.ExternalApi;
using Services.Interfaces;

namespace Services.Services.Components;

public interface IProjectService
{
    Task<Result> CreateProjectAsync(ProjectModel project);
    Task<Result<IEnumerable<ProjectDto>>> GetAllProjectsAsync();
}

public class ProjectService : BaseService, IProjectService
{
    private readonly IProjectApi _projectApi;

    public ProjectService(
        IResponseErrorHandler responseErrorHandler,
        IProjectApi projectApi)
        : base(responseErrorHandler)
    {
        _projectApi = projectApi;
    }

    public async Task<Result<IEnumerable<ProjectDto>>> GetAllProjectsAsync()
    {
        var response = await _projectApi.GetProjectsAsync();

        return ResponseErrorHandler.HandleResponse(response);
    }

    public async Task<Result> CreateProjectAsync(ProjectModel project)
    {
        var response = await _projectApi.CreateProjectAsync(project);

        return ResponseErrorHandler.HandleResponse(response);
    }
}
