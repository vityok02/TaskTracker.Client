using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface IProjectMemberService
{
    Task<Result> CreateProjectMemberAsync(Guid projectId, ProjectMemberModel model);

    Task<Result<IEnumerable<ProjectMemberDto>>> GetProjectMembersAsync(Guid projectId);

    Task<Result> UpdateMemberAsync(Guid projectId, Guid memberId, ProjectMemberModel model);

    Task<Result> DeleteMemberAsync(Guid projectId, Guid memberId);
}
