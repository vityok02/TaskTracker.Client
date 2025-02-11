using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Services.ExternalApi;

namespace Client.Components.Modules.Projects;

[Authorize]
public sealed partial class List
{
    private readonly IProjectApi _api;
    private IEnumerable<ProjectDto> Projects = [];

    public List(IProjectApi api)
    {
        _api = api;
    }

    protected override async Task OnInitializedAsync()
    {
        var response = await _api.GetProjectsAsync();

        Projects = response.Content ?? [];
    }
}
