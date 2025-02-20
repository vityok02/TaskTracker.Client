using Domain.Abstract;

namespace Domain.Dtos;

public record ProblemDetailsDto(
    string Type,
    string Title,
    int Status,
    string Detail,
    IEnumerable<Error> Errors);
