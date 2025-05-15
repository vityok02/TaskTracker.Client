using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface ITagService
{
    Task<Result<TagDto>> CreateAsync(TagModel model, Guid projectId);

    Task<Result<TagDto>> GetAsync(Guid projectId, Guid tagId);

    Task<Result<IEnumerable<TagDto>>> GetAllAsync(Guid projectId);

    Task<Result<TagDto>> UpdateAsync(Guid projectId, Guid tagId, TagModel model);

    Task<Result> ReorderAsync(Guid projectId, Guid tagId, ReorderTagModel model);

    Task<Result> DeleteAsync(Guid projectId, Guid tagId);
}
