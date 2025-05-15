namespace Domain.Constants;

public static class Colors
{
    public static readonly Dictionary<string, string> Values = new()
    {
        { "Gray", "#6c757d" },
        { "Blue", "#0000FF" },
        { "Green", "#00FF00" },
        { "Yellow", "#FFFF00" },
        { "Red", "#FF0000" },
        { "Cyan", "#00FFFF" },
        { "Orange", "#FFA500" },
        { "Purple", "#800080" },
        { "Pink", "#FFC0CB" }
    };

    public static string GetLighterColor(string hex, double factor = 1.2)
    {
        if (string.IsNullOrWhiteSpace(hex) || !hex.StartsWith("#") || hex.Length != 7)
        {
            return hex;
        }

        int r = Convert.ToInt32(hex.Substring(1, 2), 16);
        int g = Convert.ToInt32(hex.Substring(3, 2), 16);
        int b = Convert.ToInt32(hex.Substring(5, 2), 16);

        r = Math.Min((int)(r * factor), 255);
        g = Math.Min((int)(g * factor), 255);
        b = Math.Min((int)(b * factor), 255);

        return $"#{r:X2}{g:X2}{b:X2}";
    }

    public static string GetDarkerColor(string hex, double factor = 0.8)
    {
        if (string.IsNullOrWhiteSpace(hex) || !hex.StartsWith("#") || hex.Length != 7)
        {
            return hex;
        }

        int r = Convert.ToInt32(hex.Substring(1, 2), 16);
        int g = Convert.ToInt32(hex.Substring(3, 2), 16);
        int b = Convert.ToInt32(hex.Substring(5, 2), 16);

        r = (int)(r * factor);
        g = (int)(g * factor);
        b = (int)(b * factor);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
}
