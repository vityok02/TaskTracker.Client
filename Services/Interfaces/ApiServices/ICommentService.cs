using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface ICommentService
{
    Task<Result<CommentDto>> CreateAsync(Guid projectId, Guid taskId, CommentModel model);

    Task<Result<CommentDto>> GetByIdAsync(Guid projectId, Guid taskId, Guid commentId);

    Task<Result<IEnumerable<CommentDto>>> GetAllByTaskIdAsync(Guid projectId, Guid taskId);

    Task<Result> UpdateAsync(Guid projectId, Guid taskId, Guid commentId, CommentModel model);

    Task<Result> DeleteAsync(Guid projectId, Guid taskId, Guid commentId);
}
