using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace AdvancedGlow;

public class GlowConfig : BasePluginConfig
{
    [JsonPropertyName("Command")]
    public string Command { get; set; } = "esp";

    [JsonPropertyName("EspAccessFlag")]
    public string EspAccessFlag { get; set; } = "e";

    [JsonPropertyName("GlowSettings")]
    public GlowSettings GlowSettings { get; set; } = new();

    [JsonPropertyName("ColorSettings")]
    public ColorSettings ColorSettings { get; set; } = new();

    [JsonPropertyName("DiscordSettings")]
    public DiscordSettings DiscordSettings { get; set; } = new();
}

public class GlowSettings
{
    [JsonPropertyName("GlowStyle")]
    public int GlowStyle { get; set; } = 3;

    [JsonPropertyName("GlowRange")]
    public int GlowRange { get; set; } = 5000;

    [JsonPropertyName("SoundGlowMinSpeed")]
    public float SoundGlowMinSpeed { get; set; } = 150.0f;

    [JsonPropertyName("SoundGlowMaxDistance")]
    public float SoundGlowMaxDistance { get; set; } = 1100.0f;
}

public class ColorSettings
{
    [JsonPropertyName("DefaultTerroristColor")]
    public string DefaultTerroristColor { get; set; } = "255, 50, 50, 220";

    [JsonPropertyName("DefaultCounterTerroristColor")]
    public string DefaultCounterTerroristColor { get; set; } = "50, 150, 255, 220";
}

public class DiscordSettings
{
    [JsonPropertyName("Enabled")]
    public bool Enabled { get; set; } = false;

    [JsonPropertyName("WebhookUrl")]
    public string WebhookUrl { get; set; } = "PASTE_YOUR_DISCORD_WEBHOOK_URL_HERE";

    [JsonPropertyName("EmbedTitle")]
    public string EmbedTitle { get; set; } = "Advanced ESP Logs";
}