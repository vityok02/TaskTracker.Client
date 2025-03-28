using AntDesign;
using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects;

public sealed partial class ProjectList
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int? Page { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int? PageSize { get; set; }

    private PagedList<ProjectDto> PagedProjects { get; set; } = new();

    private ProjectModel _selectedProjectModel = new();

    private bool _isEdit = false;

    private bool _formVisible = false;

    private Guid _deleteProjectId;
    private string _deleteProjectName = string.Empty;
    private bool _deleteModalVisible = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private void ShowDeleteModal(Guid projectId, string projectName)
    {
        _formVisible = false;
        _deleteProjectId = projectId;
        _deleteProjectName = projectName;
        _deleteModalVisible = true;
    }

    private void ShowCreateForm()
    {
        _selectedProjectModel = new ProjectModel();
        _isEdit = false;
        _formVisible = true;
    }

    private async Task Delete(Guid id)
    {
        var result = await ProjectService
            .DeleteAsync(id);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await LoadDataAsync();
    }

    public async Task OnPageChanged(PaginationEventArgs args)
    {
        Page = args.Page > 0
            ? args.Page
            : Page;

        PageSize = args.PageSize > 0
            ? args.PageSize
            : PageSize;

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        Page ??= 1;
        PageSize ??= 9;

        var result = await ProjectService
            .GetAllAsync(Page, PageSize);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        PagedProjects = result.Value;
    }
}
