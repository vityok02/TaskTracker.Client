using Domain.Abstract;
using Domain.Dtos;
using Refit;
using System.Text.Json;

namespace Services.Extensions;

public static class IApiResponseExtensions
{
    private static readonly Error _defaultError
        = new("UnknownError", "An error occurred. Please try again later.");

    private static readonly Error _nullResponse
        = new("NullResponse", "Response content is empty.");

    private readonly static JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static ProblemDetailsDto GetProblemDetails(this IApiResponse response)
    {
        return JsonSerializer
            .Deserialize<ProblemDetailsDto>(response.Error!.Content!,
            _options)!;
    }

    public static Result HandleResponse(this IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        var problemDetails = response.GetProblemDetails();

        var errorType = problemDetails.Type
            ?? _defaultError.Code;
        var errorDetail = problemDetails.Detail
            ?? _defaultError.Message;

        return Result.Failure(problemDetails.Errors is null
            ? new Error(errorType, errorDetail)
            : new ValidationError(errorType, errorDetail, problemDetails.Errors));
    }

    public static Result<T> HandleResponse<T>(this IApiResponse<T> response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.Content is null)
            {
                return Result<T>
                    .Failure(_nullResponse);
            }

            return Result<T>.Success(response.Content);
        }

        var problemDetails = response.GetProblemDetails();

        var errorType = problemDetails.Type
            ?? _defaultError.Code;
        var errorDetail = problemDetails.Detail
            ?? _defaultError.Message;

        return Result<T>.Failure(problemDetails.Errors is null
            ? new Error(errorType, errorDetail)
            : new ValidationError(errorType, errorDetail, problemDetails.Errors));
    }
}
