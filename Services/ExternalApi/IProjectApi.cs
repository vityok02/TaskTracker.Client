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
    Task<IApiResponse<ProjectDto>> CreateProjectAsync([Body] ProjectModel model);

    [Put("/projects/{model.Id}")]
    Task<IApiResponse> UpdateProjectAsync(ProjectModel model);

    [Delete("/projects/{id}")]
    Task<IApiResponse> DeleteProjectAsync(Guid id);
}
