using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class ZeroTwoTimeSpeed : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        Server.ExecuteCommand("host_timescale 0.2");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("host_timescale 1.0");
    }

    public override string GetEffectName => "0.2x Game Speed";
    public override string GetEffectDescription => "Sets time scale to 0.2.";
    public override string? Category => "TimeDilation";
}