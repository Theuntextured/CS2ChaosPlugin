using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChaosPlugin.Effects.Effects;

public class EnderPearls : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        GetChaosPlugin()?.RegisterEventHandler<EventDecoyStarted>(OnDecoyStart);
        foreach (var Player in Utilities.GetPlayers())
        {
            Player.GiveNamedItem("weapon_decoy");
        }
    }
    
    private HookResult OnDecoyStart(EventDecoyStarted @event, GameEventInfo GameInfo)
    {
        var Player = @event.Userid;
        Vector NewPosition = new Vector(@event.X, @event.Y, @event.Z);
        
        Player?.PlayerPawn.Value?.Teleport(NewPosition);
        Utilities.GetEntityFromIndex<CDecoyProjectile>(@event.Entityid)?.Remove();
        Player?.GiveNamedItem("weapon_decoy");
        return HookResult.Handled;
    }

    public override void EndEffect()
    {
        GetChaosPlugin()?.DeregisterEventHandler<EventDecoyStarted>(OnDecoyStart);
    }

    public override string GetEffectName => "Ender Pearls";
    public override string GetEffectDescription => "Throwing decoys will teleport you wherever they land.";
}