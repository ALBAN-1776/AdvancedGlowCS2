using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdvancedGlow.Utils;

public static class ChatUtils
{
    private static readonly Dictionary<string, string> ColorMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "default", "\x01" },
        { "white", "\x01" },
        { "red", "\x02" },
        { "darkred", "\x02" },
        { "lightpurple", "\x03" },
        { "purple", "\x03" },
        { "team", "\x03" },
        { "green", "\x04" },
        { "darkgreen", "\x04" },
        { "orange", "\x05" },
        { "lime", "\x06" },
        { "lightgreen", "\x06" },
        { "slimegreen", "\x06" },
        { "lightred", "\x07" },
        { "fadedred", "\x07" },
        { "gray", "\x08" },
        { "grey", "\x08" },
        { "silver", "\x08" },
        { "yellow", "\x09" },
        { "lightyellow", "\x09" },
        { "gold", "\x0A" },
        { "golden", "\x0A" },
        { "bronze", "\x0A" },
        { "blue", "\x0B" },
        { "lightblue", "\x0B" },
        { "darkblue", "\x0C" },
        { "navy", "\x0C" },
        { "skyblue", "\x0D" },
        { "cyan", "\x0D" },
        { "aqua", "\x0D" },
        { "magenta", "\x0E" },
        { "violet", "\x0F" },
        { "pink", "\x0F" }
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
