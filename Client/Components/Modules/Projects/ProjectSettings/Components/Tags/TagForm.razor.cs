using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings.Components.Tags;

public partial class TagForm
{
    [Inject]
    public required ITagService TagService { get; init; }

    [Inject]
    public required INotificationService NotificationService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter, EditorRequired]
    public Guid ProjectId { get; set; }

    [Parameter, EditorRequired]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    [Parameter, EditorRequired]
    public Guid TagId { get; set; }

    [Parameter]
    public EventCallback<Guid> TagIdChanged { get; set; }

    [Parameter, EditorRequired]
    public TagDto? TagDto { get; set; } = new();

    [Parameter]
    public EventCallback<TagDto> OnSubmit { get; set; }

    private TagModel _tagModel = new();

    protected override void OnParametersSet()
    {
        if (TagDto is null)
        {
            _tagModel = new TagModel();
            return;
        }


        _tagModel = new TagModel
        {
            Name = TagDto.Name,
            Color = TagDto.Color,
        };
    }

    private async Task SubmitAsync()
    {
        var result = await TagService
            .UpdateAsync(ProjectId, TagId, _tagModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        _tagModel = new();

        await OnSubmit.InvokeAsync(result.Value);

        var task1 = NotificationService.Success(new NotificationConfig()
        {
            Message = $"Tag updated successfully"
        });

        var task2 = Close();

        await Task.WhenAll(task1, task2);
    }

    private async Task Close()
    {
        await ResetTagId();
        await VisibleChanged.InvokeAsync(false);
    }

    private async Task ResetTagId()
    {
        await TagIdChanged.InvokeAsync(Guid.Empty);
    }

    private static string GetRadioButtonStyle(string color, string tagColor)
    {
        return $@"
            background: {(color + "50")};
            border: 2px solid {color};
            border-radius: 20px; 
            height: 41px;
            width: 41px";
    }
}
