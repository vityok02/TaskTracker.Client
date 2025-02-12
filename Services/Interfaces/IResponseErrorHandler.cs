using Domain.Abstract;
using Refit;

namespace Services.Interfaces;

public interface IResponseErrorHandler
{
    Result HandleResponse(IApiResponse response);
    Result<T> HandleResponse<T>(IApiResponse<T> response);
}
