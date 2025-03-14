using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class CommentService : ICommentService
{
    private readonly ICommentApi _commentApi;

    public CommentService(ICommentApi commentApi)
    {
        _commentApi = commentApi;
    }

    public async Task<Result<CommentDto>> CreateAsync(
        Guid projectId,
        Guid taskId,
        CommentModel model)
    {
        var response = await _commentApi
            .CreateAsync(projectId, taskId, model);

        return response.HandleResponse();
    }

    public async Task<Result<CommentDto>> GetByIdAsync(
        Guid projectId,
        Guid taskId,
        Guid commentId)
    {
        var response = await _commentApi
            .GetByIdAsync(projectId, taskId, commentId);

        return response.HandleResponse();
    }

    public async Task<Result<IEnumerable<CommentDto>>> GetAllByTaskIdAsync(
        Guid projectId,
        Guid taskId)
    {
        var response = await _commentApi
            .GetAllByTaskIdAsync(projectId, taskId);

        return response.HandleResponse();
    }

    public async Task<Result> UpdateAsync(
        Guid projectId,
        Guid taskId,
        Guid commentId,
        CommentModel model)
    {
        var response = await _commentApi
            .UpdateAsync(projectId, taskId, commentId, model);

        return response.HandleResponse();
    }

    public async Task<Result> DeleteAsync(
        Guid projectId,
        Guid taskId,
        Guid commentId)
    {
        var response = await _commentApi
            .DeleteAsync(projectId, taskId, commentId);

        return response.HandleResponse();
    }
}
