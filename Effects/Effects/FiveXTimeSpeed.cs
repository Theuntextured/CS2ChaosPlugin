using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class FiveXTimeSpeed : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        Server.ExecuteCommand("host_timescale 5.0");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("host_timescale 1.0");
    }

    public override string GetEffectName => "5x Game Speed";
    public override string GetEffectDescription => "Quitntuples time scale.";
    public override string? Category => "TimeDilation";
}