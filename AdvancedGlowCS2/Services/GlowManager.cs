using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using AdvancedGlow.Models;
using AdvancedGlow.Utils;
using System.Drawing;

namespace AdvancedGlow.Services;

public class GlowManager
{
    private GlowConfig _config;
    private readonly GlowEntityService _entityService;
    private readonly GlowVisibilityService _visibilityService;

    public GlowManager(GlowConfig config)
    {
        _config = config;
        _entityService = new GlowEntityService(config);
        _visibilityService = new GlowVisibilityService(config);
    }

    public void UpdateConfig(GlowConfig newConfig)
    {
        _config = newConfig;
        _entityService.UpdateConfig(newConfig);
        _visibilityService.UpdateConfig(newConfig);
        UpdateAllPlayersGlowColor();
    }

    public void CreateGlowForPlayer(CCSPlayerController player)
    {
        _entityService.CreateGlowForPlayer(player);
        UpdatePlayerGlowColor(player);
    }

    public void UpdatePlayerGlowColor(CCSPlayerController player)
    {
        GlowEntitySet? entitySet = _entityService.GetGlowEntities(player);
        if (entitySet == null || !entitySet.GlowEntity.IsValid) return;

        entitySet.GlowEntity.Glow.GlowColorOverride = player.TeamNum switch
        {
            2 => ColorUtils.ParseColor(_config.ColorSettings.DefaultTerroristColor),
            3 => ColorUtils.ParseColor(_config.ColorSettings.DefaultCounterTerroristColor),
            _ => Color.White
        };
    }

    public void UpdateAllPlayersGlowColor()
    {
        foreach (var player in _entityService.GetGlowingPlayers())
        {
            UpdatePlayerGlowColor(player);
        }
    }

    public void RemoveGlowFromPlayer(CCSPlayerController player) => _entityService.RemoveGlowFromPlayer(player);
    public void RemoveAllGlow() => _entityService.RemoveAllGlow();
    public HashSet<uint> GetAllGlowEntityIndices() => _entityService.GetAllGlowEntityIndices();
    public GlowEntitySet? GetGlowEntities(CCSPlayerController player) => _entityService.GetGlowEntities(player);
    public List<CCSPlayerController> GetGlowingPlayers() => _entityService.GetGlowingPlayers();
    public bool ShouldPlayerSeeGlow(CCSPlayerController observer, CCSPlayerController target, PlayerGlowState observerState) =>
        _visibilityService.ShouldPlayerSeeGlow(observer, target, observerState);
}