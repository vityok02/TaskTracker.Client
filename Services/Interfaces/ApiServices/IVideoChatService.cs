using Domain.Abstract;
using Domain.Dtos.Twilio;

namespace Services.Interfaces.ApiServices;

public interface IVideoChatService
{
    Task<Result<TwilioJwt>> GetTokenAsync(Guid projectId);
}
