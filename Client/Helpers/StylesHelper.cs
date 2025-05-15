using Domain.Constants;

namespace Client.Helpers;

public static class StylesHelper
{
    public static string GetTagMenuElementStyle(string color, bool isSelected = false)
    {
        var darkerColor = Colors.GetDarkerColor(color);
        var backgroundColor = $"{color}10";
        var border = $"1px solid {darkerColor}";
        var fontWeight = isSelected ? "bold" : "normal";
        var icon = isSelected ? "<span style='margin-left: 8px;'>&#10003;</span>" : "";

        return $@"
            color: {darkerColor};
            background-color: {backgroundColor};
            border: {border};
            border-radius: 8px;
            margin-block: 4px;
            padding: 6px 8px;
            cursor: pointer;
            font-weight: {fontWeight};";
    }

    public static string GetTagStyle(string color, bool isSelected = false)
    {
        var darkerColor = Colors.GetDarkerColor(color);
        var backgroundColor = $"{color}10";
        var border = $"1px solid {darkerColor}";

        return $@"
            color: {darkerColor};
            background-color: {backgroundColor};
            border: {border};
            border-radius: 6px;
            padding: 2px 6px;
            font-size: 12px;";
    }

    public static string GetStateStyle(string color)
    {
        return $@"
            border: 1px solid {Colors.GetDarkerColor(color)};
            color: {Colors.GetDarkerColor(color)};
            background: {color + "10"}";
    }
}
