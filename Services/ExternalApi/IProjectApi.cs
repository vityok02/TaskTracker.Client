using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface IProjectApi : ITaskTrackerApi
{
    [Get("/projects")]
    Task<IApiResponse<IEnumerable<ProjectDto>>> GetProjectsAsync();

    [Get("/projects/{id}")]
    Task<IApiResponse<ProjectDto>> GetProjectAsync(Guid id);

    [Post("/projects")]
    Task<IApiResponse<ProjectDto>> CreateProjectAsync(ProjectModel project);

    [Put("/projects/{id}")]
    Task<IApiResponse<ProjectDto>> UpdateProjectAsync(Guid id, ProjectModel project);
}
