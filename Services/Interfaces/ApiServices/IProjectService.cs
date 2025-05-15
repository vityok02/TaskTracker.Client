using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface IProjectService
{
    Task<Result<ProjectDto>> CreateAsync(ProjectModel project);

    Task<Result<ProjectDto>> GetAsync(Guid id);

    Task<Result<PagedList<ProjectDto>>> GetAllAsync(
        int? page = 1,
        int? pageSize = 10,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null);

    Task<Result> UpdateAsync(Guid id, ProjectModel projectModel);

    Task<Result> DeleteAsync(Guid id);
}
