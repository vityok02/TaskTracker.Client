using Domain.Dtos;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Modules.Projects.ProjectSettings.Components;

public partial class ProjectDetails
{
    [Parameter, EditorRequired]
    public required ProjectDto Project { get; set; } = new();
}
