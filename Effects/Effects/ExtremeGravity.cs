using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class ExtremeGravity : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Short;
    public override void StartEffect()
    {
        Server.ExecuteCommand("sv_gravity 3200");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("sv_gravity 800");
    }

    public override string GetEffectName => "Extreme Gravity";
    public override string GetEffectDescription => "Increases gravity to 400%";
    public override string? Category => "Gravity";
}