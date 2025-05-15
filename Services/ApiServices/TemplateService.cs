using Domain.Abstract;
using Domain.Dtos;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class TemplateService : ITemplateService
{
    private readonly ITemplateApi _templateApi;

    public TemplateService(ITemplateApi templateApi)
    {
        _templateApi = templateApi;
    }

    public async Task<Result<IEnumerable<TemplateDto>>> GetAllAsync()
    {
        var response = await _templateApi.GetAllAsync();

        return response
            .HandleResponse();
    }
}
