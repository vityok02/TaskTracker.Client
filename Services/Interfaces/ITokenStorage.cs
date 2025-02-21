namespace Services.Interfaces;

public interface ITokenStorage
{
    Task SetToken(string token);

    Task<string?> GetToken();

    Task RemoveToken();
}
