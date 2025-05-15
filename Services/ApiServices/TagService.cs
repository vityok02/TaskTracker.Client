using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class TagService : ITagService
{
    private readonly ITagApi _tagApi;

    public TagService(ITagApi tagApi)
    {
        _tagApi = tagApi;
    }

    public async Task<Result<TagDto>> CreateAsync(TagModel model, Guid projectId)
    {
        var response = await _tagApi
            .CreateAsync(projectId, model);

        return response.HandleResponse();
    }

    public async Task<Result<TagDto>> GetAsync(Guid projectId, Guid tagId)
    {
        var response = await _tagApi
            .GetAsync(projectId, tagId);

        return response.HandleResponse();
    }

    public async Task<Result<IEnumerable<TagDto>>> GetAllAsync(Guid projectId)
    {
        var response = await _tagApi
            .GetAllAsync(projectId);

        return response.HandleResponse();
    }

    public async Task<Result<TagDto>> UpdateAsync(Guid projectId, Guid tagId, TagModel model)
    {
        var response = await _tagApi
            .UpdateAsync(projectId, tagId, model);

        return response.HandleResponse();
    }

    public async Task<Result> ReorderAsync(Guid projectId, Guid tagId, ReorderTagModel model)
    {
        var response = await _tagApi
            .ReorderAsync(projectId, tagId, model);

        return response.HandleResponse();
    }

    public async Task<Result> DeleteAsync(Guid projectId, Guid tagId)
    {
        var response = await _tagApi
            .DeleteAsync(projectId, tagId);

        return response.HandleResponse();
    }
}
