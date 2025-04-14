using Client.Constants;
using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Services;

public class CommentsHubService : IAsyncDisposable
{
    private const string CommentCreatedMethod = "ReceiveCommentCreated";
    private const string CommentUpdatedMethod = "ReceiveCommentUpdated";
    private const string CommentDeletedMethod = "ReceiveCommentDeleted";
    private const string CommentsHubUrl = "https://localhost:5001/hubs/comments";
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<CommentsHubService> _logger;

    private HubConnection? _hubConnection;

    public event Action<CommentDto>? OnCommentCreated;
    public event Action<CommentDto>? OnCommentUpdated;
    public event Action<Guid>? OnCommentDeleted;

    public CommentsHubService(
        NavigationManager navigationManager,
        ILogger<CommentsHubService> logger)
    {
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
                .ToAbsoluteUri(CommentsHubUrl)
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
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    private void RegisterEventHandlers()
    {
        _hubConnection!.On<CommentDto>(CommentCreatedMethod, comment =>
        {
            OnCommentCreated?.Invoke(comment);
        });

        _hubConnection!.On<CommentDto>(CommentUpdatedMethod, comment =>
        {
            OnCommentUpdated?.Invoke(comment);
        });

        _hubConnection!.On<Guid>(CommentDeletedMethod, id =>
        {
            OnCommentDeleted?.Invoke(id);
        });
    }
}
