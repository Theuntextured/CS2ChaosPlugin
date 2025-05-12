using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class RussianRoulette : ChaosEffect
{
    public override void StartEffect()
    {
        var AlivePlayers = Utilities.GetPlayers()
            .Where(P => P.IsValid && P.Pawn.IsValid && P.Pawn.Value?.Health > 0)
            .ToList();
        if (AlivePlayers.Count == 0)
        {
            return;
        }

        var SelectedPlayer = AlivePlayers[GetChaosPlugin()?.Manager.Rand.Next(AlivePlayers.Count) ?? 0];
        SelectedPlayer.ExecuteClientCommandFromServer("kill");
    }

    public override string GetEffectName => "Russian Roulette";
    public override string GetEffectDescription => "Kills a random player. Yep. Completely random.";
}