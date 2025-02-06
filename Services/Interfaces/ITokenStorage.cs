namespace Services.Interfaces;

public interface ITokenStorage
{
    void SetToken(string token);
    string GetToken();
    void RemoveToken();
}
