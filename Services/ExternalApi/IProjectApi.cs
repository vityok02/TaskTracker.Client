using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface IProjectApi : IApi
{
    [Get("/projects")]
    Task<IApiResponse<PagedList<ProjectDto>>> GetProjectsAsync(
        [Query] int? page,
        [Query] int? pageSize);

    [Get("/projects/{id}")]
    Task<IApiResponse<ProjectDto>> GetProjectAsync(Guid id);

    [Post("/projects")]
    Task<IApiResponse<ProjectDto>> CreateProjectAsync([Body] ProjectModel model);

    [Put("/projects/{model.Id}")]
    Task<IApiResponse> UpdateProjectAsync(ProjectModel model);

    [Delete("/projects/{id}")]
    Task<IApiResponse> DeleteProjectAsync(Guid id);
}
