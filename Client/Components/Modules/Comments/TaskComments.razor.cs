using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.ApiServices;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Comments;

public partial class TaskComments
{
    [Inject]
    public required ICommentService CommentService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public Guid TaskId { get; set; }

    public CommentModel CommentModel { get; set; } = new();

    private List<CommentDto> Comments { get; set; } = [];

    private CommentModel UpdateCommentModel { get; set; } = new();

    private Guid? UpdateCommentId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var commentsResult = await CommentService
            .GetAllByTaskIdAsync(ProjectId, TaskId);

        if (commentsResult.IsFailure)
        {
            AppState.ErrorMessage = commentsResult.Error!.Code;
        }

        Comments = commentsResult.Value.ToList();
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

        Comments.Add(result.Value);

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

        var commentToRemove = Comments
            .SingleOrDefault(c => c.Id == id);

        if (commentToRemove is not null)
        {
            Comments.Remove(commentToRemove);
        }
    }

    private string GetCommentDate(CommentDto comment)
    {
        return comment.UpdatedAt?.ToString("yyyy-MM-dd | HH:mm")
            ?? comment.CreatedAt.ToString("yyyy-MM-dd | HH:mm");
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

        var comment = Comments
            .FirstOrDefault(c => c.Id == UpdateCommentId);

        if (comment is not null)
        {
            comment.Comment = UpdateCommentModel.Comment;
            comment.UpdatedAt = DateTime.Now;
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
}
