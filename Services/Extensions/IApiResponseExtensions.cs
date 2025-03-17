using Domain.Abstract;
using Domain.Constants;
using Domain.Dtos;
using Refit;
using System.Text.Json;

namespace Services.Extensions;

public static class IApiResponseExtensions
{
    private static readonly Error _defaultError
        = new("UnknownError", "An error occurred. Please try again later.");

    private readonly static JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static ProblemDetailsDto GetProblemDetails(this IApiResponse response)
    {
        if (string.IsNullOrEmpty(response.Error?.Content))
        {
            return new ProblemDetailsDto
            {
                Type = _defaultError.Code,
                Detail = _defaultError.Message
            };
        }

        try
        {
            return JsonSerializer
                .Deserialize<ProblemDetailsDto>(response.Error.Content, _options)
                    ?? new ProblemDetailsDto
                    {
                        Type = _defaultError.Code,
                        Detail = _defaultError.Message
                    };
        }
        catch (Exception)
        {
            return new ProblemDetailsDto
            {
                Type = _defaultError.Code,
                Detail = _defaultError.Message
            };
        }
    }

    public static Result HandleResponse(this IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        return MapResponseError(response);
    }

    public static Result<T> HandleResponse<T>(this IApiResponse<T> response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.Content is null)
            {
                return Result<T>
                    .Failure(_defaultError);
            }

            return Result<T>.Success(response.Content);
        }

        return MapResponseError(response);
    }

    private static Error MapResponseError(IApiResponse response)
    {
        var problemDetails = response.GetProblemDetails();

        var errorType = problemDetails.Type
            ?? _defaultError.Code;

        return problemDetails?.Type?.Split('.').Last() switch
        {
            ErrorTypes.ValidationError
                => new ValidationError(
                    errorType,
                    "Please check your input data",
                    problemDetails.Errors),

            ErrorTypes.InvalidCredentials
                => new Error(
                    errorType,
                    "Invalid email or password"),

            ErrorTypes.InvalidToken
                => new Error(
                    errorType,
                    "Invalid token"),

            ErrorTypes.Unauthorized
                => new Error(
                    errorType,
                    "You are not authorized to perform this action"),

            ErrorTypes.NotFound
                => new Error(
                    errorType,
                    "Resource not found"),

            ErrorTypes.Conflict
                => new Error(
                    errorType,
                    "Resource already exists"),

            _ => _defaultError
        };
    }
}
