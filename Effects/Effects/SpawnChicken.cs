using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;

namespace ChaosPlugin.Effects.Effects;

public class SpawnChicken : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            if (Player == null || !Player.IsValid || Player.Pawn == null || !Player.Pawn.IsValid)
                continue;

            var Pawn = Player.Pawn.Value;

            // Create the chicken entity
            var ChickenEntity = Utilities.CreateEntityByName<CChicken>("chicken");
            if (ChickenEntity == null || !ChickenEntity.IsValid)
                continue;

            // Set the chicken's position to the player's location
            ChickenEntity.Teleport(Pawn?.AbsOrigin, Pawn?.AbsRotation, Pawn?.AbsVelocity);

            // Spawn the chicken into the world
            ChickenEntity.DispatchSpawn();
            ChickenEntity.Leader.Raw = Player.Pawn.Raw;
            ChickenEntity.ActiveFollowStartTime = Server.CurrentTime;
        }
    }

    public override string GetEffectName => "Spawn Chicken";
    public override string GetEffectDescription => "Spawns a chicken";
}