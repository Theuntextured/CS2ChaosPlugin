using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class RandomizeHealth : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            var Pawn = Player.PlayerPawn.Value;
            if(Pawn == null) continue;
            Pawn.Health = int.Max(1, (int)(Pawn.MaxHealth * Rand.NextDouble()));
            Utilities.SetStateChanged(Pawn, "CBaseEntity", "m_iHealth");
        }
    }

    public override string GetEffectName => "Randomize Health";
    public override string GetEffectDescription => "Randomizes health for all players.";
    public override string? Category => "Health";
}