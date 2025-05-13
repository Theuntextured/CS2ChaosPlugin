using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChaosPlugin.Effects.Effects;

public class ShuffleLocations : ChaosEffect
{
    public override void StartEffect()
    {
        var Players   = Utilities.GetPlayers()
            .Where(p => p.IsValid 
                        && p.PlayerPawn.IsValid 
                        && p.PlayerPawn.Value?.Health > 0)
            .ToList();
        var Positions = Players
            .Select(p => new Vector(p.PlayerPawn.Value?.AbsOrigin?.X ?? 0, p.PlayerPawn.Value?.AbsOrigin?.Y ?? 0, p.PlayerPawn.Value?.AbsOrigin?.Z ?? 0))
            .ToList();
        Positions.Shuffle();

        ExplodeInMidair.GraceTime = 0.5f;
        for (int i = 0; i < Players.Count; i++)
        {
            var Pawn = Players[i].PlayerPawn.Value;
            Pawn?.Teleport(Positions[i]);
        }

    }

    public override string GetEffectName => "Shuffle Locations";
    public override string GetEffectDescription => "Shuffles locations of players.";
}