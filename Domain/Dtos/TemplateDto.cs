namespace Domain.Dtos;

public record TemplateDto(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder);
