using Client.Constants;
using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Client.Services;

public class CommentsHubService : IAsyncDisposable
{
    private readonly CommentsHubOptions _hubOptions;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<CommentsHubService> _logger;

    private HubConnection? _hubConnection;

    public event Action<CommentDto>? OnCommentCreated;
    public event Action<CommentDto>? OnCommentUpdated;
    public event Action<Guid>? OnCommentDeleted;

    public CommentsHubService(
        IOptions<CommentsHubOptions> hubOptions,
        NavigationManager navigationManager,
        ILogger<CommentsHubService> logger)
    {
        _hubOptions = hubOptions.Value;
        _navigationManager = navigationManager;
        _logger = logger;
    }

    public async Task StartConnection()
    {
        if (_hubConnection is not null)
        {
            return;
        }

        var hubUrl = Environment
            .GetEnvironmentVariable(EnvironmentKeys.CommentsHubUrl)
            ?? _navigationManager
                .ToAbsoluteUri(_hubOptions.HubUrl)
                .ToString();

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        RegisterEventHandlers();

        try
        {
            await _hubConnection.StartAsync();
            _logger.LogInformation("SignalR connection started to {Url}", hubUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Error starting SignalR connection to {Url}. Message: {Message}",
                hubUrl,
                ex.Message);
        }
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    private void RegisterEventHandlers()
    {
        _hubConnection!.On<CommentDto>(_hubOptions.CreatedMethod, comment =>
        {
            OnCommentCreated?.Invoke(comment);
        });

        _hubConnection!.On<CommentDto>(_hubOptions.UpdatedMethod, comment =>
        {
            OnCommentUpdated?.Invoke(comment);
        });

        _hubConnection!.On<Guid>(_hubOptions.DeletedMethod, id =>
        {
            OnCommentDeleted?.Invoke(id);
        });
    }
}
