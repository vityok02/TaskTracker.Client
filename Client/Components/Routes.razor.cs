namespace Client.Components;

public partial class Routes
{
    private ApplicationState? ApplicationState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ApplicationState ??= new ApplicationState();

        await base.OnInitializedAsync();
    }
}
