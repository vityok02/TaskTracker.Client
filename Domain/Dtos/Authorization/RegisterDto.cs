using Domain.Dtos.Authorization;

namespace Domain.Dtos;

public record RegisterDto(string Id, TokenDto Token);
