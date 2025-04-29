using Domain.Dtos.Twilio;
using Refit;

namespace Services.ExternalApi;

public interface IVideoChatApi : IApi
{
    [Get("/projects/{projectId}/videochat/token")]
    Task<IApiResponse<TwilioJwt>> GetTokenAsync(Guid projectId);
}
