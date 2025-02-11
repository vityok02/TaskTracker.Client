namespace Domain.Abstract;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static implicit operator Result(Error error) => Result.Failure(error);

    public override string ToString()
    {
        return $"{Code}. {Message}";
    }
}

public sealed record ValidationError(string Code, string Message, IEnumerable<Error> Errors)
    : Error(Code, Message);
