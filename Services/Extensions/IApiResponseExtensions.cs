using Domain.Dtos;
using Refit;
using System.Text.Json;

namespace Services.Extensions;

public static class IApiResponseExtensions
{
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
}
