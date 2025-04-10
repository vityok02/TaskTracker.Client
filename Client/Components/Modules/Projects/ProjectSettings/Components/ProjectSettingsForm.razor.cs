using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Modules.Projects.ProjectSettings.Components;

public partial class ProjectSettingsForm
{
    [Parameter, EditorRequired]
    public ProjectModel ProjectModel { get; set; } = new();

    [Parameter, EditorRequired]
    public EventCallback OnSubmitSuccess { get; set; }

    private async Task UpdateAsync()
    {
        await OnSubmitSuccess.InvokeAsync();
    }
}
