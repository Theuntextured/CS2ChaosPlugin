using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Memory;

namespace ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

public static class ChaosUtilities
{
    public static void SpawnExplosion(Vector Origin, CCSPlayerController? Thrower = null, int Magnitude = 100, int Radius = 250)
    {
        CBasePlayerPawn? ThrowerPawn = null;
        if (Thrower != null)
        {
            ThrowerPawn = Thrower.Pawn.Value;
        }
        var Grenade = Utilities.CreateEntityByName<CHEGrenadeProjectile>("hegrenade_projectile");
        if (Grenade == null) return;    
        Grenade.Damage = Magnitude;
        Grenade.DmgRadius = Radius;
        if (ThrowerPawn != null) Grenade.TeamNum = ThrowerPawn.TeamNum;
        Grenade.Teleport(Origin);
        Grenade.DispatchSpawn();
        if (Thrower != null) Grenade.Thrower.Raw = Thrower.Pawn.Raw;
        Grenade.AcceptInput("InitializeSpawnFromWorld", ThrowerPawn, ThrowerPawn);
        Grenade.DetonateTime = 0;
    }
    
    public static void Shuffle<T>(this List<T> List)
    {
        int n = List.Count;
        Random Rng = new Random();
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            (List[k], List[n]) = (List[n], List[k]);
        }
    }

    public static void RemoveWeaponsExceptKnife(CCSPlayerController Player)
    {
        if (!Player.IsValid || !Player.Pawn.IsValid)
            return;
        
        // Remove existing weapons
        Player.RemoveWeapons();
        Server.NextFrame(() =>
        {
            Player.GiveNamedItem("weapon_knife");
        });
    }
}