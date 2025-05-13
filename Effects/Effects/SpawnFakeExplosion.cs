using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChaosPlugin.Effects.Effects;

public class SpawnFakeExplosion : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            ChaosUtilities.SpawnExplosion(Player.Pawn.Value?.AbsOrigin ?? new Vector(), Player, 0);
        }
    }

    public override string GetEffectName => "Spawn Fake Explosion";
    public override string GetEffectDescription => "Spawns an explosion that does no damage. Scared you didn't it?";
}