using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface IProjectService
{
    Task<Result> CreateProjectAsync(ProjectModel project);

    Task<Result<ProjectDto>> GetProjectAsync(Guid id);

    Task<Result<IEnumerable<ProjectDto>>> GetAllProjectsAsync();

    Task<Result> UpdateProjectAsync(ProjectModel projectModel);

    Task<Result> DeleteProjectAsync(Guid id);
}
