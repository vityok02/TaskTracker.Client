using Domain.Abstract;

namespace Client.Extensions;

public static class ResultExtensions
{
    //public static void HandleResult(
    //    this Result result,
    //    ApplicationState appState)
    //{
    //    if (result.IsSuccess)
    //    {
    //        return;
    //    }

    //    if (result.Error is ValidationError validationError)
    //    {
    //        appState.SetError(validationError.Errors
    //            .FirstOrDefault()?.Message
    //                ?? "Unknown validation error");

    //        return;
    //    }

    //    appState.SetError(result.Error?.Message
    //        ?? string.Empty);
    //}

    //public static T HandleResult<T>(
    //    this Result<T> result,
    //    ApplicationState appState)
    //{
    //    if (result.IsSuccess)
    //    {
    //        return result.Value;
    //    }

    //    if (result.Error is ValidationError validationError)
    //    {
    //        appState.SetError(validationError.Errors
    //            .FirstOrDefault()?.Message
    //                ?? "Unknown validation error");

    //        return result.Value;
    //    }

    //    appState.SetError(result.Error?.Message
    //        ?? string.Empty);

    //    return result.Value;
    //}
}
