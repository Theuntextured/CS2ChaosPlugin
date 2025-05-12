using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class NoGravity : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Short;
    public override void StartEffect()
    {
        Server.ExecuteCommand("sv_gravity 0");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("sv_gravity 800");
    }

    public override string GetEffectName => "No Gravity";
    public override string GetEffectDescription => "Lowers gravity to 0%";
    public override string? Category => "Gravity";
}