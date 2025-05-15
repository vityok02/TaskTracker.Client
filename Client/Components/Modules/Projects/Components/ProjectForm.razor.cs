using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.Components;

public sealed partial class ProjectForm
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [Inject]
    public required ITemplateService TemplateService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required INotificationService Notification { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    [Parameter]
    public EventCallback OnProjectSaved { get; set; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; set; }

    [Parameter]
    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    private List<TemplateDto> Templates { get; set; } = [];

    private bool IsEmptySelected => ProjectModel.TemplateId == Guid.Empty;

    protected override async Task OnInitializedAsync()
    {
        var result = await TemplateService
            .GetAllAsync();

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        Templates = result.Value
            .ToList();
    }

    private async Task Submit()
    {
        var result = await ProjectService
                .CreateAsync(ProjectModel);

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        await OnProjectSaved.InvokeAsync();

        await VisibleChanged.InvokeAsync(false);

        _ = Notification.Success(new NotificationConfig()
        {
            Message = "Project successfully created!"
        });

        NavigationManager.NavigateTo($"/projects/{result.Value.Id}/tasks");
    }

    private void Cancel()
    {
        VisibleChanged.InvokeAsync(false);
    }

    private string GetIcon(string template)
    {
        return template switch
        {
            "Basic Kanban" => "bi-kanban",
            "Software Development" => "bi-code-slash",
            _ => "bi-box-seam"
        };
    }
}