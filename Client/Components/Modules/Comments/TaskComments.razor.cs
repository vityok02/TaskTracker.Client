using Client.Extensions;
using Client.Services;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Comments;

public partial class TaskComments : IAsyncDisposable
{
    [Inject]
    public required ICommentService CommentService { get; init; }

    [Inject]
    public required CommentsHubService CommentHubService { get; init; }

    [Inject]
    public required AuthenticationStateProvider StateProvider { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public Guid TaskId { get; set; }

    private Guid UserId { get; set; }

    private CommentModel CommentModel { get; set; } = new();

    private List<CommentDto> Comments { get; set; } = [];

    private CommentModel UpdateCommentModel { get; set; } = new();

    private Guid? UpdateCommentId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        CommentHubService.OnCommentCreated += HandleCommentReceived;
        CommentHubService.OnCommentUpdated += HandleUpdatedComment;
        CommentHubService.OnCommentDeleted += HandleDeletedComment;

        await CommentHubService.StartConnection();

        var commentsResult = await CommentService
            .GetAllByTaskIdAsync(ProjectId, TaskId);

        if (commentsResult.IsFailure)
        {
            AppState.ErrorMessage = commentsResult.Error!.Code;
        }

        Comments = commentsResult.Value
            .ToList();

        UserId = (await StateProvider
            .GetUserIdAsync()).Value;
    }

    private async Task CreateCommentAsync()
    {
        var result = await CommentService
            .CreateAsync(ProjectId, TaskId, CommentModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        CommentModel = new CommentModel();
    }

    private async Task DeleteCommentAsync(Guid id)
    {
        var result = await CommentService
            .DeleteAsync(ProjectId, TaskId, id);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }
    }

    private string GetCommentDate(CommentDto comment)
    {
        return comment.UpdatedAt?.ToLocalTime().ToString("yyyy-MM-dd | HH:mm")
            ?? comment.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd | HH:mm");
    }

    private async Task UpdateCommentAsync()
    {
        if (UpdateCommentId is null)
        {
            return;
        }

        var result = await CommentService
            .UpdateAsync(ProjectId, TaskId, UpdateCommentId.Value, UpdateCommentModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        DeleteUpdateCommentId();
    }

    private void SetUpdateCommentId(Guid id)
    {
        var comment = Comments
            .FirstOrDefault(c => c.Id == id);

        if (comment is null)
        {
            return;
        }

        UpdateCommentModel.Comment = comment.Comment;
        UpdateCommentId = id;
    }

    private void DeleteUpdateCommentId()
    {
        UpdateCommentId = null;
    }

    public async ValueTask DisposeAsync()
    {
        CommentHubService.OnCommentCreated -= HandleCommentReceived;
        await CommentHubService.DisposeAsync();
    }

    private bool IsUsersComment(CommentDto comment)
    {
        return UserId == comment.CreatedBy.Id;
    }

    private void HandleCommentReceived(CommentDto commentDto)
    {
        Comments.Add(commentDto);
        InvokeAsync(StateHasChanged);
    }

    private void HandleUpdatedComment(CommentDto commentDto)
    {
        var index = Comments
            .FindIndex(c => c.Id == commentDto.Id);

        if (index != -1)
        {
            Comments[index] = commentDto;
            InvokeAsync(StateHasChanged);
        }
    }

    private void HandleDeletedComment(Guid id)
    {
        Comments.RemoveAll(c => c.Id == id);
        InvokeAsync(StateHasChanged);
    }
}
