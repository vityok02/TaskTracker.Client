using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.ProjectMembers.Components;

public partial class ProjectMemberForm
{
    [Inject]
    public required IUserService UserService { get; init; }

    [Inject]
    public required IProjectMemberService ProjectMemberService { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public IEnumerable<RoleDto> Roles { get; set; } = [];

    [Parameter]
    public EventCallback OnMemberAdded { get; set; }

    public ProjectMemberModel MemberModel { get; set; } = new();

    public string SearchedMemberName { get; set; } = string.Empty;

    public IEnumerable<UserDto> SearchedUsers { get; set; } = [];

    private CancellationTokenSource _cts = new();

    private async Task SearchUser()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        await Task.Delay(500, token)
            .ContinueWith(t => { }, TaskContinuationOptions.OnlyOnRanToCompletion);

        if (string.IsNullOrEmpty(SearchedMemberName))
        {
            SearchedUsers = [];
            return;
        }

        var result = await UserService
            .SearchUsersAsync(SearchedMemberName);

        if (token.IsCancellationRequested)
        {
            return;
        }

        if (result.IsSuccess)
        {
            SearchedUsers = result.Value;
        }
    }

    private async Task AddMember()
    {
        var selectedMember = SearchedUsers
            .FirstOrDefault(m => m.Username == SearchedMemberName);

        MemberModel.UserId = selectedMember?.Id
            ?? Guid.Empty;

        if (!CanAddMember())
        {
            return;
        }

        await ProjectMemberService
            .CreateProjectMemberAsync(ProjectId, MemberModel);

        await OnMemberAdded.InvokeAsync();

        SearchedUsers = [];

        SearchedMemberName = string.Empty;
    }
    private bool CanAddMember()
    {
        return MemberModel.UserId != Guid.Empty
            && MemberModel.RoleId != Guid.Empty;
    }
}
