﻿using System.Drawing;
using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class WallHack : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;

    public Dictionary</*player slot*/int, 
        Tuple</*prop 1*/CBaseModelEntity, /*prop 2*/CBaseModelEntity>> glowingPlayers = new();
    
    public override void StartEffect()
    {
        SetAllPlayersGlowing();
    }

    public override void EndEffect()
    {
        RemoveAllGlowingPlayers();
    }
    
    //Source: https://github.com/aquevadis/bg-koka-cs2-xray-esp/blob/main/AdminESP/AdminESP.cs
    
    public void SetAllPlayersGlowing() {
        var cachedPlayers = Utilities.GetPlayers();
        for (int i = 0; i < cachedPlayers.Count(); i++) {

            //skip invalid and dead players when assigning prop
            if (cachedPlayers[i] is null || cachedPlayers[i].IsValid is not true || cachedPlayers[i].PawnIsAlive is not true) continue;

            SetPlayerGlowing(cachedPlayers[i], cachedPlayers[i].TeamNum);

        }

    }

    public void RemoveAllGlowingPlayers() {

        foreach (var glowingProp in glowingPlayers.Values)
        {
            if (glowingProp.Item1 is not null && glowingProp.Item1.IsValid is true
                                              && glowingProp.Item2 is not null && glowingProp.Item2.IsValid is true) {
                
                //remove previous modelRelay prop
                glowingProp.Item1.AcceptInput("Kill");
                //remove previous modelGlow prop
                glowingProp.Item2.AcceptInput("Kill");
            }
        }
    }
    
    public void SetPlayerGlowing(CCSPlayerController player, int team)
    {

        if (player is null || player.IsValid is not true 
        || player.Connected is not PlayerConnectedState.PlayerConnected) return;

        var playerPawn = player.PlayerPawn.Value;
        if (playerPawn is null || playerPawn.IsValid is not true) return;

        CBaseModelEntity? modelGlow = Utilities.CreateEntityByName<CBaseModelEntity>("prop_dynamic");
        CBaseModelEntity? modelRelay = Utilities.CreateEntityByName<CBaseModelEntity>("prop_dynamic");

        if (modelGlow is null || modelRelay is null  
        || modelGlow.IsValid is not true || modelRelay.IsValid is not true) return;

        var playerCBodyComponent = playerPawn.CBodyComponent;
        if (playerCBodyComponent is null) return;

        var playerSceneMode = playerCBodyComponent.SceneNode;
        if (playerSceneMode is null) return;

        string modelName = playerSceneMode.GetSkeletonInstance().ModelState.ModelName;

        modelRelay.SetModel(modelName);
        modelRelay.Spawnflags = 256u;
        modelRelay.RenderMode = RenderMode_t.kRenderNone;
        modelRelay.DispatchSpawn();

        modelGlow.SetModel(modelName);
        modelGlow.Spawnflags = 256u;
        modelGlow.DispatchSpawn();

        switch (team) {
            case 2:
                modelGlow.Glow.GlowColorOverride = Color.Orange; //T
            break;
            case 3:
                modelGlow.Glow.GlowColorOverride = Color.SkyBlue; //CT
            break;
        }
        
        modelGlow.Glow.GlowRange = 5000;
        modelGlow.Glow.GlowTeam = -1;
        modelGlow.Glow.GlowType = 3;
        modelGlow.Glow.GlowRangeMin = 100;

        modelRelay.AcceptInput("FollowEntity", playerPawn, modelRelay, "!activator");
        modelGlow.AcceptInput("FollowEntity", modelRelay, modelGlow, "!activator");

        //if player already has glowing metadata remove previous one before adding new one
        if (glowingPlayers.ContainsKey(player.Slot) is true) {

            if (glowingPlayers[player.Slot].Item1 is not null && glowingPlayers[player.Slot].Item1.IsValid is true
            && glowingPlayers[player.Slot].Item2 is not null && glowingPlayers[player.Slot].Item2.IsValid is true) {
                
                //remove previous modelRelay prop
                glowingPlayers[player.Slot].Item1.AcceptInput("Kill");
                //remove previous modelGlow prop
                glowingPlayers[player.Slot].Item2.AcceptInput("Kill");
            }

            //remove player from the list
            glowingPlayers.Remove(player.Slot);
        }

        //add player to the list
        glowingPlayers.Add(player.Slot, new Tuple<CBaseModelEntity, CBaseModelEntity>(modelRelay,modelGlow));

    }

    public override string GetEffectName => "WallHack";
    public override string GetEffectDescription => "30k Premiere experience.";
}