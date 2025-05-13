using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class InfiniteAmmo : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Long;
    public override void StartEffect()
    {
        Server.ExecuteCommand("sv_infinite_ammo 1");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("sv_infinite_ammo 0");
    }

    public override string GetEffectName => "Infinite Ammo";
    public override string GetEffectDescription => "Provides with infinite ammo.";
}