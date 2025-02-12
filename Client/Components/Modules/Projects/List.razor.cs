using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Services.Services.Components;

namespace Client.Components.Modules.Projects;

[Authorize]
public sealed partial class List
{
    private readonly IProjectService _projectService;
    private IEnumerable<ProjectDto> Projects = [];

    public List(IProjectService projectService)
    {
        _projectService = projectService;
    }

    protected override async Task OnInitializedAsync()
    {
        var result = await _projectService.GetAllProjectsAsync();

        if (result.IsFailure)
        {
            return;
        }

        Projects = result.Value;
    }
}
