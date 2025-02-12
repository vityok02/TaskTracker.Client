using Services.ExternalApi;
using Services.Interfaces;

namespace Services.Services.Components;

public abstract class BaseService
{
    protected readonly IResponseErrorHandler ResponseErrorHandler;
    
    protected BaseService(
        IResponseErrorHandler responseErrorHandler)
    {
        ResponseErrorHandler = responseErrorHandler;
    }
}
