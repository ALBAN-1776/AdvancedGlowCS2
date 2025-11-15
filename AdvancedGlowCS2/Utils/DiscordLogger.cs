using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AdvancedGlow.Models;

namespace AdvancedGlow.Utils;

public static class DiscordLogger
{
    private static readonly HttpClient HttpClient = new();

    public static async Task LogCommandUsageAsync(DiscordSettings settings, string adminName, string command, string result, bool isEnableAction)
    {
        if (!settings.Enabled || string.IsNullOrWhiteSpace(settings.WebhookUrl) || settings.WebhookUrl.Contains("ВАШ_URL"))
            return;

        int embedColor = isEnableAction ? 3066993 : 15158332;

        var payload = new WebhookPayload
        {
            Embeds = new List<Embed>
            {
                new Embed
                {
                    Title = settings.EmbedTitle,
                    Description = $"Администратор **{adminName}** использовал команду.",
                    Color = embedColor,
                    Fields = new List<EmbedField>
                    {
                        new EmbedField { Name = "Команда", Value = $"`{command}`", Inline = true },
                        new EmbedField { Name = "Результат", Value = result, Inline = true }
                    },
                    Footer = new EmbedFooter { Text = "Advanced ESP" },
                    Timestamp = DateTime.UtcNow
                }
            }
        };

        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            await HttpClient.PostAsync(settings.WebhookUrl, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Advanced ESP] Discord log failed: {ex.Message}");
        }
    }
}