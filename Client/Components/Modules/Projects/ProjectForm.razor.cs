using Domain.Models;
using Microsoft.AspNetCore.Components;
namespace Client.Components.Modules.Projects;

public sealed partial class ProjectForm : ComponentBase
{
    [Parameter]
    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    [Parameter]
    public EventCallback OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private async Task Submit()
    {
        await OnSubmit.InvokeAsync();
    }

    private async Task Cancel()
    {
        await OnCancel.InvokeAsync();
    }
}