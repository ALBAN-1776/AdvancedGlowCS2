using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API;
using IksAdminApi;
using AdvancedGlow.Services;
using AdvancedGlow.Utils;   

namespace AdvancedGlow;

public class AdvancedGlow : BasePlugin, IPluginConfig<GlowConfig>
{
    public override string ModuleName => "Advanced ESP by. ALBAN1776";
    public override string ModuleVersion => "1.1.0";
    public override string ModuleAuthor => "ALBAN1776";

    public required GlowConfig Config { get; set; }
    private GlowManager _glowManager = null!;
    private readonly Dictionary<ulong, PlayerGlowState> _playerStates = new();

    public override void Load(bool hotReload)
    {
        Console.WriteLine("[Advanced ESP] Плагин успешно загружен.");

        _glowManager = new GlowManager(Config);
        AddCommand(Config.Command, "Toggle player glow visibility", OnGlowCommand);
        RegisterListener<Listeners.CheckTransmit>(OnCheckTransmit);
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn, HookMode.Post);
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Post);
        RegisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Post);
    }

    public override void Unload(bool hotReload)
    {
        if (_glowManager != null)
        {
            _glowManager.RemoveAllGlow();
        }
    }

    public void OnConfigParsed(GlowConfig newConfig)
    {
        Config = newConfig;

        if (_glowManager != null)
        {
            _glowManager.UpdateConfig(newConfig);
            _glowManager.UpdateAllPlayersGlowColor();
        }
    }

    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    private void OnGlowCommand(CCSPlayerController player, CommandInfo command)
    {
        if (player == null) return;

        if (command.ArgCount < 2)
        {
            player.PrintToColorChat($"{{YELLOW}}Использование: {{LIME}}!{Config.Command} <all|enemies|team|sound|visible>");
            return;
        }

        if (!_playerStates.ContainsKey(player.SteamID))
        {
            _playerStates[player.SteamID] = new PlayerGlowState();
        }

        var state = _playerStates[player.SteamID];
        var arg = command.GetArg(1).ToLower();
        string resultMessage = "";
        bool isEnablingAction = false;

        switch (arg)
        {
            case "all":
            case "enemies":
            case "team":
            case "sound":
            case "visible":

                var admin = player.Admin();

                if (admin == null || (!admin.CurrentFlags.Contains(Config.EspAccessFlag) && !admin.CurrentFlags.Contains("z")))
                {
                    player.PrintToColorChat("{RED}У вас нет прав для использования этой команды.");
                    return;
                }

                GlowMode previousMode = state.CurrentMode;
                GlowMode targetMode = arg switch
                {
                    "all" => GlowMode.All,
                    "enemies" => GlowMode.Enemies,
                    "team" => GlowMode.Teammates,
                    "sound" => GlowMode.Sound,
                    "visible" => GlowMode.OnlyVisible,
                    _ => GlowMode.Disabled
                };

                if (previousMode == targetMode)
                {
                    state.CurrentMode = GlowMode.Disabled;
                    resultMessage = "Свечение отключено";
                    isEnablingAction = false;
                }
                else
                {
                    state.CurrentMode = targetMode;
                    resultMessage = $"Свечение для '{arg}' включено";
                    isEnablingAction = true;
                }

                string modeText = state.CurrentMode switch
                {
                    GlowMode.All => "Все игроки",
                    GlowMode.Enemies => "Враги",
                    GlowMode.Teammates => "Союзники",
                    GlowMode.Sound => "Шумящие враги",
                    GlowMode.OnlyVisible => "Видимые враги",
                    _ => "Отключено"
                };

                player.PrintToColorChat($"Режим свечения: {(state.CurrentMode != GlowMode.Disabled ? $"{{GREEN}}{modeText}" : "{RED}Отключено")}");
                _ = DiscordLogger.LogCommandUsageAsync(Config.DiscordSettings, player.PlayerName, $"!{Config.Command} {arg}", resultMessage, isEnablingAction);
                break;

            default:
                player.PrintToColorChat($"{{YELLOW}}Использование: {{LIME}}!{Config.Command} <all|enemies|team|sound|visible>");
                break;
        }
    }

    private void OnCheckTransmit(CCheckTransmitInfoList infoList)
    {
        var glowEntityIndices = _glowManager.GetAllGlowEntityIndices();
        if (!glowEntityIndices.Any()) return;

        foreach (var (info, observer) in infoList)
        {
            if (observer == null || !observer.IsValid || !observer.Pawn.IsValid || observer.IsBot)
                continue;

            if (!_playerStates.TryGetValue(observer.SteamID, out var observerState))
            {
                foreach (var index in glowEntityIndices)
                {
                    info.TransmitEntities.Remove(index);
                }
                continue;
            }

            foreach (var target in _glowManager.GetGlowingPlayers())
            {
                if (!_glowManager.ShouldPlayerSeeGlow(observer, target, observerState))
                {
                    var targetEntities = _glowManager.GetGlowEntities(target);
                    if (targetEntities != null)
                    {
                        if (targetEntities.GlowEntity.IsValid)
                            info.TransmitEntities.Remove(targetEntities.GlowEntity.Index);
                        if (targetEntities.GlowRelay.IsValid)
                            info.TransmitEntities.Remove(targetEntities.GlowRelay.Index);
                    }
                }
            }
        }
    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !player.IsValid)
            return HookResult.Continue;

        AddTimer(0.2f, () =>
        {
            if (player.IsValid && player.PawnIsAlive)
            {
                _glowManager.CreateGlowForPlayer(player);
            }
        });
        return HookResult.Continue;
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player != null && player.IsValid)
        {
            _glowManager.RemoveGlowFromPlayer(player);
        }
        return HookResult.Continue;
    }

    private HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        _glowManager.RemoveAllGlow();
        return HookResult.Continue;
    }
}