using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.ProjectMembers.Components;

public partial class ProjectMemberList
{
    [Inject]
    public required IProjectMemberService ProjectMemberService { get; init; }

    [Inject]
    public required INotificationService NotificationService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public IEnumerable<ProjectMemberDto> Members { get; set; } = [];

    [Parameter]
    public IEnumerable<RoleDto> Roles { get; set; } = [];

    [Parameter]
    public EventCallback OnMemberChanged { get; set; }

    private async Task UpdateMemberAsync(Guid roleId, Guid userId)
    {
        var result = await ProjectMemberService
            .UpdateMemberAsync(ProjectId, userId, new ProjectMemberModel { RoleId = roleId });

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await NotificationService.Success(new NotificationConfig()
        {
            Message = "Member updated successfully"
        });
    }

    private async Task DeleteMemberAsync(Guid memberId)
    {
        var result = await ProjectMemberService
            .DeleteMemberAsync(ProjectId, memberId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await OnMemberChanged.InvokeAsync();

        await NotificationService.Success(new NotificationConfig()
        {
            Message = "Member deleted successfully"
        });
    }
}
