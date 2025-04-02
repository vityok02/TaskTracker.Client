using Blazored.LocalStorage;
using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Services.Interfaces.ApiServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Components.Modules.User;

public sealed partial class Profile
{
    [Inject]
    public required IUserService UserService { get; init; }

    [Inject]
    public required ILocalStorageService Storage { get; init; }

    [Inject]
    public required AuthenticationStateProvider AuthStateProvider { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Inject]
    public required NavigationManager NavManager { get; init; }

    [Parameter]
    public Guid UserId { get; set; }

    private bool IsAuthorized { get; set; }

    private UserDto? User { get; set; }

    private bool isFirstRender = true;

    protected override async Task OnParametersSetAsync()
    {
        var result = await UserService
            .GetUserByIdAsync(UserId);

        if (result.IsFailure)
        {
            if (isFirstRender)
            {
                AppState.ErrorMessage = result.Error!.Message;
            }

            NavManager.NavigateTo("/");
            return;
        }

        IsAuthorized = await IsUserAuthorizedAsync();

        User = result.Value;

        isFirstRender = false;
    }

    private async Task<bool> IsUserAuthorizedAsync()
    {
        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var subClaim = user
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(subClaim, out Guid tokenUserId))
            {
                return (UserId == tokenUserId);
            }
        }

        return false;
    }

    private async Task UploadFileAsync(InputFileChangeEventArgs args)
    {
        var file = args.File;

        if (!IsFileValid(file))
        {
            return;
        }

        using var content = new MultipartFormDataContent();
        {
            content.Add(new StreamContent(file.OpenReadStream(file.Size)),
                "file",
                file.Name);
        }

        var response = await UserService
            .UploadAvatarAsync(content);

        if (response.IsFailure)
        {
            AppState.ErrorMessage = response.Error!.Message;
            return;
        }

        await Storage
            .SetItemAsStringAsync("AvatarUrl", response.Value.FileUrl);

        User!.AvatarUrl = response.Value.FileUrl;

        StateHasChanged();
    }

    private bool IsFileValid(IBrowserFile file)
    {
        string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

        if (file is null)
        {
            AppState.ErrorMessage = "No file selected.";
            return false;
        }

        const long maxFileSize = 10 * 1024 * 1024;

        var isValidExtension = allowedExtensions
            .Contains(Path.GetExtension(file.Name));

        if (!isValidExtension)
        {
            AppState.ErrorMessage = "Such file type is not allowed.";
            return false;
        }

        if (file.Size > maxFileSize)
        {
            AppState.ErrorMessage = "The file exceeds the maximum allowed size of 10 MB.";
            return false;
        }

        return true;
    }
}