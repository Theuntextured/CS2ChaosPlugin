using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class OneOneXTimeSpeed : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        Server.ExecuteCommand("host_timescale 1.1");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("host_timescale 1.0");
    }

    public override string GetEffectName => "1.1x Game Speed";
    public override string GetEffectDescription => "1.1x time scale.";
    public override string? Category => "TimeDilation";
}