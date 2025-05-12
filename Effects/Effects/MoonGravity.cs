using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class MoonGravity : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Long;
    public override void StartEffect()
    {
        Server.ExecuteCommand("sv_gravity 200");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("sv_gravity 800");
    }

    public override string GetEffectName => "Moon Gravity";
    public override string GetEffectDescription => "Lowers gravity to 25%";
    public override string? Category => "Gravity";
}