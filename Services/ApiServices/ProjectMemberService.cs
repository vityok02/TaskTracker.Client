using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class ProjectMemberService : IProjectMemberService
{
    private readonly IProjectMemberApi _projectMemberApi;

    public ProjectMemberService(IProjectMemberApi projectMemberApi)
    {
        _projectMemberApi = projectMemberApi;
    }

    public async Task<Result> CreateProjectMemberAsync(Guid projectId, ProjectMemberModel model)
    {
        var response = await _projectMemberApi
            .CreateAsync(projectId, model);

        return response.HandleResponse();
    }

    public async Task<Result<IEnumerable<ProjectMemberDto>>> GetProjectMembersAsync(Guid projectId)
    {
        var response = await _projectMemberApi
            .GetAllAsync(projectId);

        return response.HandleResponse();
    }

    public async Task<Result> UpdateMemberAsync(Guid projectId, Guid memberId, ProjectMemberModel model)
    {
        var response = await _projectMemberApi
            .UpdateAsync(projectId, memberId, model);

        return response.HandleResponse();
    }

    public async Task<Result> DeleteMemberAsync(Guid projectId, Guid memberId)
    {
        var response = await _projectMemberApi
            .DeleteAsync(projectId, memberId);

        return response.HandleResponse();
    }
}
