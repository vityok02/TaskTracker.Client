using Domain.Abstract;

namespace Domain.Dtos;

public class ProblemDetailsDto
{
    public string Type { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public int Status { get; init; }

    public string Detail { get; init; } = string.Empty;

    public IEnumerable<Error> Errors { get; init; } = [];
}