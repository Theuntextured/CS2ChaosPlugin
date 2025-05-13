using System.Drawing;
using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class Invisibility : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Short;
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            var Pawn = Player.PlayerPawn.Value;
            if(Pawn == null) continue;
            Pawn.Render = Color.Transparent;
            Utilities.SetStateChanged(Pawn, "CBaseModelEntity", "m_clrRender");
        }
    }

    public override void EndEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            var Pawn = Player.PlayerPawn.Value;
            if(Pawn == null) continue;
            Pawn.Render = Color.White;
            Utilities.SetStateChanged(Pawn, "CBaseModelEntity", "m_clrRender");
        }
    }

    public override string GetEffectName => "Invisibility";
    public override string GetEffectDescription => "Everyone is invisible.";
    public override string? Category => "PlayerRender";
}