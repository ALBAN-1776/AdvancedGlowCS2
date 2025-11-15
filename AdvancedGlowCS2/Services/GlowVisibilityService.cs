using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace AdvancedGlow.Services;

public class GlowVisibilityService
{
    private GlowConfig _config;

    public GlowVisibilityService(GlowConfig config)
    {
        _config = config;
    }

    public void UpdateConfig(GlowConfig newConfig)
    {
        _config = newConfig;
    }

    public bool ShouldPlayerSeeGlow(CCSPlayerController observer, CCSPlayerController target, PlayerGlowState observerState)
    {
        if (observerState.CurrentMode == GlowMode.Disabled) return false;
        if (observer.SteamID == target.SteamID) return false;

        switch (observerState.CurrentMode)
        {
            case GlowMode.All:
                return true;
            case GlowMode.Enemies:
                return observer.TeamNum != target.TeamNum;
            case GlowMode.Teammates:
                return observer.TeamNum == target.TeamNum;
            case GlowMode.Sound:
                if (observer.TeamNum == target.TeamNum) return false;

                var targetPawn = target.PlayerPawn.Value;
                var observerPawn = observer.PlayerPawn.Value;
                if (targetPawn == null || !targetPawn.IsValid || observerPawn == null || !observerPawn.IsValid) return false;

                bool isShooting = (target.Buttons & PlayerButtons.Attack) != 0;
                bool isRunning = targetPawn.AbsVelocity.Length2D() > _config.GlowSettings.SoundGlowMinSpeed;
                bool isScoping = new CCSPlayerPawn(targetPawn.Handle).IsScoped;
                bool isJumping = (target.Buttons & PlayerButtons.Jump) != 0;
                bool isReloading = (target.Buttons & PlayerButtons.Reload) != 0;
                bool isAiming = (target.Buttons & PlayerButtons.Attack2) != 0;

                bool isInAir = (targetPawn.Flags & (uint)PlayerFlags.FL_ONGROUND) == 0;
                bool isMovingInAir = isInAir && targetPawn.AbsVelocity.Length() > 50.0f;

                bool isMakingNoise = isRunning || isShooting || isScoping || isJumping || isReloading || isAiming || isMovingInAir;

                if (!isMakingNoise) return false;

                if (observerPawn.AbsOrigin == null || targetPawn.AbsOrigin == null) return false;
                float distance = (targetPawn.AbsOrigin - observerPawn.AbsOrigin).Length();

                return distance <= _config.GlowSettings.SoundGlowMaxDistance;

            case GlowMode.OnlyVisible:
                if (observer.TeamNum == target.TeamNum) return false;

                var targetPawnForRadar = target.PlayerPawn.Value;
                if (targetPawnForRadar == null || !targetPawnForRadar.IsValid) return false;

                bool isSpotted = false;
                try
                {
                    var spottedMask = targetPawnForRadar.EntitySpottedState.SpottedByMask;
                    if (spottedMask != null && spottedMask.Length > 0)
                    {
                        int observerSlot = observer.Slot;
                        int arrayIndex = observerSlot / 32;
                        int bitIndex = observerSlot % 32;

                        if (arrayIndex < spottedMask.Length)
                        {
                            uint mask = spottedMask[arrayIndex];
                            isSpotted = (mask & (1u << bitIndex)) != 0;
                        }
                    }
                }
                catch
                {
                    isSpotted = false;
                }
                return isSpotted;

            default:
                return false;
        }
    }
}