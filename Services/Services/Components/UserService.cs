namespace Services.Services.Components;

public class UserService
{
    private readonly HttpClient _client;

    public UserService(HttpClient client)
    {
        _client = client;
    }

    //public Task<IEnumerable<UserDto>> GetUsers()
    //{

    //    //var users = _client.
    //}
}
