using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace ChaosPlugin.Effects.Effects;

public class EveryoneIsInvincible : ChaosEffect
{
    private List<CCSPlayerController> InvinciblePlayers = [];

    private HookResult OnBombBeginPlant(EventBombBeginplant @event, GameEventInfo info)
    {
        var Player = @event.Userid;
        if (Player != null && Player.IsValid)
        {
            InvinciblePlayers.Remove(Player);
        }
        return HookResult.Continue;
    }
    private HookResult OnBombAbortPlant(EventBombAbortplant @event, GameEventInfo info)
    {
        var Player = @event.Userid;
        if (Player != null && Player.IsValid)
        {
            InvinciblePlayers.Add(Player);
        }
        return HookResult.Continue;
    }
    private HookResult OnBombPlanted(EventBombPlanted @event, GameEventInfo info)
    {
        var Player = @event.Userid;
        if (Player != null && Player.IsValid)
        {
            InvinciblePlayers.Add(Player);
        }
        return HookResult.Continue;
    }
    
    private HookResult OnBombBeginDefuse(EventBombBegindefuse @event, GameEventInfo info)
    {
        var Player = @event.Userid;
        if (Player != null && Player.IsValid)
        {
            InvinciblePlayers.Remove(Player);
        }
        return HookResult.Continue;
    }
    private HookResult OnBombAbortDefuse(EventBombAbortdefuse @event, GameEventInfo info)
    {
        var Player = @event.Userid;
        if (Player != null && Player.IsValid)
        {
            InvinciblePlayers.Add(Player);
        }
        return HookResult.Continue;
    }
    private HookResult OnBombDiffused(EventBombDefused @event, GameEventInfo info)
    {
        var Player = @event.Userid;
        if (Player != null && Player.IsValid)
        {
            InvinciblePlayers.Add(Player);
        }
        return HookResult.Continue;
    }
    
    private HookResult OnTakeDamageOldFunc(DynamicHook arg)
    {
        var Victim = arg.GetParam<CBaseEntity>(0);
        if (Victim.DesignerName != "player")
            return HookResult.Continue;
        
        var Controller = Victim.As<CCSPlayerPawn>().Controller.Value;
        if (Controller == null)
            return HookResult.Continue;

        return InvinciblePlayers.Contains(Controller) ? HookResult.Handled : HookResult.Continue;
    }

    public override ChaosEffectDuration Duration => ChaosEffectDuration.Short;
    public override void StartEffect()
    {
        GetChaosPlugin()?.RegisterEventHandler<EventBombBeginplant>(OnBombBeginPlant);
        GetChaosPlugin()?.RegisterEventHandler<EventBombAbortplant>(OnBombAbortPlant);
        GetChaosPlugin()?.RegisterEventHandler<EventBombPlanted>(OnBombPlanted);

        GetChaosPlugin()?.RegisterEventHandler<EventBombBegindefuse>(OnBombBeginDefuse);
        GetChaosPlugin()?.RegisterEventHandler<EventBombAbortdefuse>(OnBombAbortDefuse);
        GetChaosPlugin()?.RegisterEventHandler<EventBombDefused>(OnBombDiffused);

        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamageOldFunc, HookMode.Pre);
        
        InvinciblePlayers = Utilities.GetPlayers();
    }

    public override void EndEffect()
    {
        GetChaosPlugin()?.DeregisterEventHandler<EventBombBeginplant>(OnBombBeginPlant);
        GetChaosPlugin()?.DeregisterEventHandler<EventBombAbortplant>(OnBombAbortPlant);
        GetChaosPlugin()?.DeregisterEventHandler<EventBombPlanted>(OnBombPlanted);

        GetChaosPlugin()?.DeregisterEventHandler<EventBombBegindefuse>(OnBombBeginDefuse);
        GetChaosPlugin()?.DeregisterEventHandler<EventBombAbortdefuse>(OnBombAbortDefuse);
        GetChaosPlugin()?.DeregisterEventHandler<EventBombDefused>(OnBombDiffused);

        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamageOldFunc, HookMode.Pre);
    }

    public override string GetEffectName => "Everyone Is Invincible";
    public override string GetEffectDescription => "Everyone is invincible, except for bomb planter and diffuser.";
}