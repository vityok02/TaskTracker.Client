namespace Domain.Constants;

public class ErrorTypes
{
    public readonly string ValidationError = "ValidationError";
    public readonly string NotFound = "NotFound";
    public const string Unauthorized = "Unauthorized";
    public const string InvalidCredentials = "InvalidCredentials";
    public const string Conflict = "AlreadyExists";
    public const string InvalidToken = "InvalidToken";
}
