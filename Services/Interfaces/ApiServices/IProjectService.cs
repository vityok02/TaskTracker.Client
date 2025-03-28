using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface IProjectService
{
    Task<Result> CreateAsync(ProjectModel project);

    Task<Result<ProjectDto>> GetAsync(Guid id);

    Task<Result<PagedList<ProjectDto>>> GetAllAsync(int? page = 1, int? pageSize = 10);

    Task<Result> UpdateAsync(ProjectModel projectModel);

    Task<Result> DeleteAsync(Guid id);
}
