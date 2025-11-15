using CounterStrikeSharp.API.Core;

namespace AdvancedGlow.Models;
public class GlowEntitySet
{
    public CBaseModelEntity GlowRelay { get; }
    public CBaseModelEntity GlowEntity { get; }
    public GlowEntitySet(CBaseModelEntity relay, CBaseModelEntity entity)
    {
        GlowRelay = relay;
        GlowEntity = entity;
    }
}