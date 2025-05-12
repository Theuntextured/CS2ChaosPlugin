using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChaosPlugin.Effects.Effects;

public class CallOfDuty : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Long;
    public override void StartEffect()
    {
        ChaosPlugin.Plugin?.RegisterEventHandler<EventWeaponFire>(EventDiceNoRecoilOnWeaponFire);
        //ConVar.Find("weapon_accuracy_nospread")?.SetValue(true);
        Server.ExecuteCommand("weapon_accuracy_nospread 1");
    }

    private HookResult EventDiceNoRecoilOnWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;
        if (player == null
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || player.PlayerPawn.Value.WeaponServices == null
            || player.PlayerPawn.Value.WeaponServices.ActiveWeapon == null
            || !player.PlayerPawn.Value.WeaponServices.ActiveWeapon.IsValid
            || player.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value == null) return HookResult.Continue;
        CBasePlayerWeapon weapon = player.PlayerPawn!.Value!.WeaponServices!.ActiveWeapon!.Value!;
        // reset playerpawn recoil
        player.PlayerPawn.Value.AimPunchAngle.X = 0;
        player.PlayerPawn.Value.AimPunchAngle.Y = 0;
        player.PlayerPawn.Value.AimPunchAngle.Z = 0;
        player.PlayerPawn.Value.AimPunchAngleVel.X = 0;
        player.PlayerPawn.Value.AimPunchAngleVel.Y = 0;
        player.PlayerPawn.Value.AimPunchAngleVel.Z = 0;
        player.PlayerPawn.Value.AimPunchTickBase = -1;
        player.PlayerPawn.Value.AimPunchTickFraction = 0;
        //decrease recoil
        weapon.As<CCSWeaponBase>().FlRecoilIndex = 0;
        return HookResult.Continue;
    }

    public override void EndEffect()
    {
        ChaosPlugin.Plugin?.DeregisterEventHandler<EventWeaponFire>(EventDiceNoRecoilOnWeaponFire);
        //ConVar.Find("weapon_accuracy_nospread")?.SetValue(false);
        Server.ExecuteCommand("weapon_accuracy_nospread 0");
    }
    
    public override string GetEffectName => "Call Of Duty";
    public override string GetEffectDescription => "No spread, no recoil.";
}