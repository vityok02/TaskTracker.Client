using AntDesign;
using Microsoft.AspNetCore.Components;

namespace Client.Extensions;

public static class RenderFragments
{
    public static RenderFragment DeleteIcon => builder =>
    {
        builder.OpenComponent<Icon>(0);
        builder.AddAttribute(1, "Type", IconType.Outline.Delete);
        builder.CloseComponent();
    };
}
