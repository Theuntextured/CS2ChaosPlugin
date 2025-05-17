using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class DoubleTimeSpeed : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        Server.ExecuteCommand("host_timescale 2.0");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("host_timescale 1.0");
    }

    public override string GetEffectName => "2x Game Speed";
    public override string GetEffectDescription => "Doubles time scale.";
    public override string? Category => "TimeDilation";
}