using System.Drawing;
using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class RainbowPlayers : ChaosEffect
{
    private List<CCSPlayerController> Players = [];
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        Players = Utilities.GetPlayers();
    }

    public override void EndEffect()
    {
        foreach (var Player in Players)
        {
            var Pawn = Player.PlayerPawn.Value;
            if(Pawn == null) continue;
            Pawn.Render = Color.White;
            Utilities.SetStateChanged(Pawn, "CBaseModelEntity", "m_clrRender");
        }
    }

    public override void TickEffect(float Dt)
    {
        foreach (var Player in Players)
        {
            var ColorToUse = ChaosUtilities.HsvToRgb((Server.CurrentTime + Player.Index) * 100 % 360f, 1, 1);
            var Pawn = Player.PlayerPawn.Value;
            if(Pawn == null) continue;
            Pawn.Render = ColorToUse;
            Utilities.SetStateChanged(Pawn, "CBaseModelEntity", "m_clrRender");
        }
        
    }

    public override string GetEffectName => "Rainbow Players";
    public override string GetEffectDescription => "Players turn into a 13-year-old's bedroom.";
    public override string? Category => "PlayerRender";
}