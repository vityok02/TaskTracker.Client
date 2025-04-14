namespace Client;

public class CommentsHubOptions
{
    public string HubUrl { get; set; } = string.Empty;

    public string CreatedMethod { get; set; } = string.Empty;

    public string UpdatedMethod { get; set; } = string.Empty;

    public string DeletedMethod { get; set; } = string.Empty;
}
