using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChaosPlugin.Effects.Effects;

public class ExplosiveCombat : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override string GetEffectName => "ExplosiveCombat";
    public override string GetEffectDescription => "Guns shoot explosive rounds.";

    public override void StartEffect()
    {
        if (ChaosPlugin.Plugin == null) return;
        ChaosPlugin.Plugin.RegisterEventHandler<EventBulletImpact>(OnBulletImpact);
    }
    
    
    private HookResult OnBulletImpact(EventBulletImpact Event, GameEventInfo Info)
    {
        var Player = Event.Userid;
        if (Player == null || !Player.IsValid)
            return HookResult.Continue;

        var HitPosition = new Vector(Event.X, Event.Y, Event.Z);
        ChaosUtilities.SpawnExplosion(HitPosition, Player, 100, 250);

        return HookResult.Continue;
    }

    public override void EndEffect()
    {
        if (ChaosPlugin.Plugin == null) return;
        
        ChaosPlugin.Plugin.DeregisterEventHandler<EventBulletImpact>(OnBulletImpact);
    }
}