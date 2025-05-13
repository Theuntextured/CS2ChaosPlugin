using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class OHKO : ChaosEffect
{
    private List<Tuple<CBasePlayerPawn, int>> OriginalHealths = [];
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            var Pawn = Player.Pawn.Value;
            if(Pawn == null) continue;
            OriginalHealths.Add(new Tuple<CBasePlayerPawn, int>(Pawn, Pawn.Health));

            Pawn.Health = 1;
            Pawn.MaxHealth = 1;
            
            Utilities.SetStateChanged(Pawn, "CBaseEntity", "m_iHealth");
            Utilities.SetStateChanged(Pawn, "CBaseEntity", "m_iMaxHealth");
        }
        
        GetChaosPlugin()?.RegisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        OriginalHealths.Clear();
        
        foreach (var Player in Utilities.GetPlayers())
        {
            var Pawn = Player.Pawn.Value;
            if(Pawn == null) continue;
            OriginalHealths.Add(new Tuple<CBasePlayerPawn, int>(Pawn, 100));

            Pawn.Health = 1;
            Pawn.MaxHealth = 1;
            
            Utilities.SetStateChanged(Pawn, "CBaseEntity", "m_iHealth");
            Utilities.SetStateChanged(Pawn, "CBaseEntity", "m_iMaxHealth");
        }
        
        return HookResult.Continue;
    }

    public override void EndEffect()
    {
        foreach (var Pawn in OriginalHealths)
        {
            Pawn.Item1.Health = Pawn.Item2;
            Pawn.Item1.MaxHealth = 100;
            
            Utilities.SetStateChanged(Pawn.Item1, "CBaseEntity", "m_iHealth");
            Utilities.SetStateChanged(Pawn.Item1, "CBaseEntity", "m_iMaxHealth");
        }
        
        GetChaosPlugin()?.DeregisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    public override string GetEffectName => "OHKO";

    public override string GetEffectDescription =>
        "One Hit KO: Everyone is 1 HP temporarily. Survive and you will get hour HP back.";
    public override string? Category => "Health";
}