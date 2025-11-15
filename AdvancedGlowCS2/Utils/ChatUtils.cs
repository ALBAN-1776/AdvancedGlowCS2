using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdvancedGlow.Utils;

public static class ChatUtils
{
    private static readonly Dictionary<string, string> ColorMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "RED", "\x02" }, { "GREEN", "\x04" }, { "LIME", "\x06" }, { "BLUE", "\x0B" },
        { "YELLOW", "\x09" }, { "PURPLE", "\x03" }, { "GRAY", "\x08" }, { "ORANGE", "\x05" },
        { "DEFAULT", "\x01" }, { "WHITE", "\x01" }
    };

    private static readonly Regex ColorPattern = new Regex(@"\{([A-Za-z]+)\}", RegexOptions.Compiled);

    public static string ProcessColors(string message)
    {
        if (string.IsNullOrEmpty(message)) return message;
        return ColorPattern.Replace(" \x01" + message, match =>
        {
            string colorName = match.Groups[1].Value;
            return ColorMap.TryGetValue(colorName, out var colorCode) ? colorCode : match.Value;
        });
    }

    public static void PrintToColorChat(this CCSPlayerController player, string message)
    {
        if (player == null || !player.IsValid) return;
        player.PrintToChat(ProcessColors(message));
    }
}

public static class ColorUtils
{
    public static Color ParseColor(string colorString)
    {
        var parts = colorString.Split(',');
        if (parts.Length != 4) return Color.White;

        if (byte.TryParse(parts[0], out var r) &&
            byte.TryParse(parts[1], out var g) &&
            byte.TryParse(parts[2], out var b) &&
            byte.TryParse(parts[3], out var a))
        {
            return Color.FromArgb(a, r, g, b);
        }
        return Color.White;
    }
}