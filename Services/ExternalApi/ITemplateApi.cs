using Domain.Dtos;
using Refit;

namespace Services.ExternalApi;

public interface ITemplateApi : IApi
{
    [Get("/templates")]
    Task<IApiResponse<IEnumerable<TemplateDto>>> GetAllAsync();
}
