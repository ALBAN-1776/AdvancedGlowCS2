namespace AdvancedGlow;

public enum GlowMode { Disabled, All, Enemies, Teammates, Sound, OnlyVisible }

public class PlayerGlowState
{
    public GlowMode CurrentMode { get; set; } = GlowMode.Disabled;
}