using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class ProjectService : IProjectService
{
    private readonly IProjectApi _projectApi;

    public ProjectService(IProjectApi projectApi)
    {
        _projectApi = projectApi;
    }

    public async Task<Result<PagedList<ProjectDto>>> GetAllAsync(
        int? page, int? pageSize)
    {
        var response = await _projectApi
            .GetProjectsAsync(page, pageSize);

        return response
            .HandleResponse();
    }

    public async Task<Result> CreateAsync(
        ProjectModel project)
    {
        var response = await _projectApi
            .CreateProjectAsync(project);

        return response
            .HandleResponse();
    }

    public async Task<Result<ProjectDto>> GetAsync(Guid id)
    {
        var response = await _projectApi
            .GetProjectAsync(id);

        return response
            .HandleResponse();
    }

    public async Task<Result> UpdateAsync(ProjectModel projectModel)
    {
        var response = await _projectApi
            .UpdateProjectAsync(projectModel);

        return response
            .HandleResponse();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var response = await _projectApi
            .DeleteProjectAsync(id);

        return response
            .HandleResponse();
    }
}
