using Domain.Abstract;
using Refit;
using Services.Extensions;
using Services.Interfaces;

namespace Services.Services;

public class ResponseErrorHandler : IResponseErrorHandler
{
    private readonly Error _defaultError
        = new("UnknownError", "An error occurred. Please try again later.");
    private readonly Error _nullResponse
        = new("NullResponse", "Response content is empty.");

    public Result HandleResponse(IApiResponse response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = response.GetProblemDetails();

            var errorType = problemDetails.Type
                ?? _defaultError.Code;
            var errorDetail = problemDetails.Detail
                ?? _defaultError.Message;

            return Result.Failure(problemDetails.Errors is null
                ? new Error(errorType, errorDetail)
                : new ValidationError(errorType, errorDetail, problemDetails.Errors));
        }

        return Result.Success();
    }

    public Result<T> HandleResponse<T>(IApiResponse<T> response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = response.GetProblemDetails();

            var errorType = problemDetails.Type
                ?? _defaultError.Code;
            var errorDetail = problemDetails.Detail
                ?? _defaultError.Message;

            return Result<T>.Failure(problemDetails.Errors is null
                ? new Error(errorType, errorDetail)
                : new ValidationError(errorType, errorDetail, problemDetails.Errors));
        }

        if (response.Content is null)
        {
            return Result<T>
                .Failure(_nullResponse);
        }

        return Result<T>.Success(response.Content);
    }
}