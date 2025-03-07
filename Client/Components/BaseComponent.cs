using Domain.Abstract;
using Microsoft.AspNetCore.Components;

namespace Client.Components;

public partial class BaseComponent : ComponentBase
{
    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    protected async Task HandleRequest<T>(Func<Task<Result<T>>> request, Action<T> action)
    {
        var result = await request();
        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        action(result.Value);
    }

    protected async Task HandleRequest(Func<Task<Result>> request)
    {
        var result = await request();

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
        }
    }
}
