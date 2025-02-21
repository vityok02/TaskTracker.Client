using Services.Interfaces;
using System.Net.Http.Headers;

namespace Client.Authentication;

public class AuthHttpClientHandler : DelegatingHandler
{
    private readonly ITokenStorage _storage;

    public AuthHttpClientHandler(ITokenStorage tokenStorage)
    {
        _storage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _storage.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}