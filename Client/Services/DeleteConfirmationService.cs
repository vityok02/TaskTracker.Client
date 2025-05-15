using AntDesign;
using Client.Extensions;
using Microsoft.AspNetCore.Components;

namespace Client.Services;

public class DeleteConfirmationService
{
    private readonly ModalService _modalService;

    public DeleteConfirmationService(
        ModalService modalService)
    {
        _modalService = modalService;
    }

    public async Task ShowStateDeleteConfirmAsync(
        string fieldName,
        Guid stateId,
        Func<Guid, Task> deleteAction)
    {
        await _modalService.ConfirmAsync(new ConfirmOptions()
        {
            Title = "Delete confirmation",
            Content = GetContentFragment(fieldName),
            Icon = RenderFragments.DeleteIcon,
            OnOk = async (e) => await deleteAction(stateId),
            OnCancel = (e) => Task.CompletedTask,
            OkText = "Delete",
            CancelText = "Cancel",
            OkButtonProps = new ButtonProps
            {
                Danger = true
            }
        });
    }

    private static RenderFragment GetContentFragment(string fieldName)
    {
        return builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, $"This will permanently delete this option from the \"{fieldName}\" field.");

            builder.OpenElement(2, "span");
            builder.AddAttribute(3, "class", "fw-bold");
            builder.AddContent(4, " This cannot be undone.");
            builder.CloseElement();

            builder.CloseElement();

            builder.OpenElement(5, "div");
            builder.AddAttribute(6, "class", "custom-description bg-light border border-danger text-danger p-2 rounded mt-2");
            builder.OpenElement(7, "span");
            builder.AddAttribute(8, "class", "text-danger");
            builder.AddContent(9, "Warning:");
            builder.CloseElement();
            builder.AddContent(10, " The option will be permanently deleted from any items in this project.");
            builder.CloseElement();
        };
    }
}
