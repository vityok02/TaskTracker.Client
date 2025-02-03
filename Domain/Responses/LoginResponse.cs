namespace Domain.Responses;

public record LoginResponse(string Token, double ExpiresIn);
