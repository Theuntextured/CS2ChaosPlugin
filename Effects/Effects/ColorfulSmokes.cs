﻿using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class ColorfulSmokes : ChaosEffect
{
    private Listeners.OnEntitySpawned? SmokeListener;
    
    public override void StartEffect()
    {
        if (GetChaosPlugin() == null) return;

        SmokeListener = Entity =>
        {
            if (Entity.DesignerName != "smokegrenade_projectile") return;

            var Projectile = new CSmokeGrenadeProjectile(Entity.Handle);

            // Changes smoke grenade color to a random color each time.
            Server.NextFrame(() =>
            {
                Projectile.SmokeColor.X = Random.Shared.NextSingle() * 255.0f;
                Projectile.SmokeColor.Y = Random.Shared.NextSingle() * 255.0f;
                Projectile.SmokeColor.Z = Random.Shared.NextSingle() * 255.0f;
            });
        };

        GetChaosPlugin()?.RegisterListener(SmokeListener);
    }
    public override string GetEffectName => "Colorful Smokes";
    public override string GetEffectDescription => "Makes smokes colorful!";

    public override void EndEffect()
    {
        if(GetChaosPlugin() == null) return;
        if(SmokeListener == null) return;
        GetChaosPlugin()?.RemoveListener(SmokeListener);
    }

    public override ChaosEffectDuration Duration => ChaosEffectDuration.Long;
}