using System.Text.Json.Serialization;

namespace AdvancedGlow.Models;

public class WebhookPayload
{
    [JsonPropertyName("embeds")]
    public List<Embed>? Embeds { get; set; }
}

public class Embed
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("color")]
    public int Color { get; set; }

    [JsonPropertyName("fields")]
    public List<EmbedField>? Fields { get; set; }

    [JsonPropertyName("footer")]
    public EmbedFooter? Footer { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

public class EmbedField
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("value")]
    public string Value { get; set; } = "";

    [JsonPropertyName("inline")]
    public bool Inline { get; set; }
}

public class EmbedFooter
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}