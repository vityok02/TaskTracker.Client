using Domain.Abstract;
using Domain.Dtos;

namespace Services.Interfaces.ApiServices;

public interface ITemplateService
{
    Task<Result<IEnumerable<TemplateDto>>> GetAllAsync();
}
