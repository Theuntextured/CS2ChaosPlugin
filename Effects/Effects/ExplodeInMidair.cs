using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class ExplodeInMidair : ChaosEffect
{
    public static float GraceTime = 0.5f;
    public override void StartEffect()
    {
        GraceTime = 0.5f;
    }

    public override void TickEffect(float Dt)
    {
        var Players = Utilities.GetPlayers();
        GraceTime -= Dt;
        if(GraceTime > 0) return;

        
        foreach (var Player in Players)
        {
            var Pawn = Player.PlayerPawn.Value;
            if(Pawn == null) continue;
            if(Pawn.Health <= 0) continue;
            if (Pawn.GroundEntity.Value == null && Pawn.AbsOrigin != null && Pawn.IsValid) //if player is in air and position is valid
            {
                ChaosUtilities.SpawnExplosion(Pawn.AbsOrigin, Player);
            }
        }
    }

    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override string GetEffectName => "Explode in Midair";
    public override string GetEffectDescription => "Will cause you to explode when not on the ground or on ladders.";
}