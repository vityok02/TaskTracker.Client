using Microsoft.AspNetCore.Components;

namespace Client.Components;

public partial class BaseComponent : ComponentBase
{
    [Inject]
    public required NavigationManager NavManager { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }
}

