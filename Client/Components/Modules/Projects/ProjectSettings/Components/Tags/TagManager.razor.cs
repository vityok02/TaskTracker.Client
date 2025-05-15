using Client.Helpers;
using Client.Services;
using Domain.Constants;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings.Components.Tags;

public partial class TagManager
{
    [Inject]
    public required ITagService TagService { get; init; }

    [Inject]
    public required DeleteConfirmationService DeleteStateConfirmationService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public required Guid ProjectId { get; init; }

    private List<TagDto> Tags { get; set; } = [];

    private TagModel TagModel { get; set; } = new();

    private Guid SelectedTagId { get; set; }

    private bool FormVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await TagService
            .GetAllAsync(ProjectId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Tags = result.Value
            .OrderBy(t => t.SortOrder)
            .ToList();
    }

    private static string GetTagTitleStyle(string color)
    {
        return $@"
            border: 1px solid {Colors.GetDarkerColor(color)};
            color: {Colors.GetDarkerColor(color)};
            background: {color + "10"}";
    }

    private void ShowUpdateForm(Guid tagId)
    {
        FormVisible = true;
        SelectedTagId = tagId;
    }

    public void UpdateTag(TagDto updatedTag)
    {
        var index = Tags
            .FindIndex(s => s.Id == updatedTag.Id);

        Tags[index] = updatedTag;
    }

    private async Task ShowDeleteConfirmationAsync(Guid stateId)
    {
        await DeleteStateConfirmationService
            .ShowStateDeleteConfirmAsync("Tag", stateId, DeleteAsync);
    }

    private async Task DeleteAsync(Guid tagId)
    {
        var result = await TagService
            .DeleteAsync(ProjectId, tagId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Tags.RemoveAll(t => t.Id == tagId);
    }

    private async Task CreateTagAsync()
    {
        var result = await TagService
            .CreateAsync(TagModel, ProjectId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Tags.Add(result.Value);

        TagModel = new();
    }

    private async Task ReorderAsync((int oldIndex, int lastIndex) indices)
    {
        var (oldIndex, newIndex) = indices;

        var items = Tags;
        var itemToMove = items[oldIndex];

        ReorderListHelper.Reorder(items, oldIndex, newIndex);
        var belowItem = ReorderListHelper.GetBelow(items, newIndex);

        var reorderTagModel = new ReorderTagModel
        {
            BeforeTagId = belowItem?.Id,
        };

        var result = await TagService
            .ReorderAsync(ProjectId, itemToMove.Id, reorderTagModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }
    }
}
