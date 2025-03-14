namespace Domain.Models;

public class CommentModel
{
    public Guid Id { get; set; }

    public string Comment { get; set; } = string.Empty;
}
