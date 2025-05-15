using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Components.Modules.Tasks.Components;

public partial class TaskTagList
{
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public TaskDto Task { get; set; }

    private List<TagDto> VisibleTags = new();
    private List<TagDto> HiddenTags = new();
    private ElementReference TagContainerRef;

    protected override void OnInitialized()
    {
        VisibleTags = Task.Tags.ToList();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Task.Tags.Count > 0)
        {
            var indexes = await JSRuntime.InvokeAsync<int[]>("visibleTagIndexes");

            VisibleTags = new();
            HiddenTags = new();

            for (int i = 0; i < Task.Tags.Count; i++)
            {
                if (indexes.Contains(i))
                    VisibleTags.Add(Task.Tags[i]);
                else
                    HiddenTags.Add(Task.Tags[i]);
            }

            StateHasChanged();
        }
    }
}
