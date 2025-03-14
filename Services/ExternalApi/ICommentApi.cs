using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface ICommentApi : IApi
{
    [Post("/projects/{projectId}/tasks/{taskId}/comments")]
    Task<IApiResponse<CommentDto>> CreateAsync(Guid projectId, Guid taskId, [Body] CommentModel comment);

    [Get("/projects/{projectId}/tasks/{taskId}/comments/{commentId}")]
    Task<IApiResponse<CommentDto>> GetByIdAsync(Guid projectId, Guid taskId, Guid commentId);

    [Get("/projects/{projectId}/tasks/{taskId}/comments")]
    Task<IApiResponse<IEnumerable<CommentDto>>> GetAllByTaskIdAsync(Guid projectId, Guid taskId);

    [Put("/projects/{projectId}/tasks/{taskId}/comments/{commentId}")]
    Task<IApiResponse> UpdateAsync(Guid projectId, Guid taskId, Guid commentId, [Body] CommentModel comment);

    [Delete("/projects/{projectId}/tasks/{taskId}/comments/{commentId}")]
    Task<IApiResponse> DeleteAsync(Guid projectId, Guid taskId, Guid commentId);
}
