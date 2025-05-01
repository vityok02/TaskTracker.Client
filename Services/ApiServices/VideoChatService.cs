using Domain.Abstract;
using Domain.Dtos.Twilio;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;
using System.ComponentModel.DataAnnotations;

namespace Services.ApiServices;

public class VideoChatService : IVideoChatService
{
    private readonly IVideoChatApi _videoChatApi;

    public VideoChatService(IVideoChatApi videoChatApi)
    {
        _videoChatApi = videoChatApi;
    }

    public async Task<Result<TwilioJwt>> GetTokenAsync(Guid projectId)
    {
        var response = await _videoChatApi
            .GetTokenAsync(projectId);

        return response.HandleResponse();
    }
}
