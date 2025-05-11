using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin;

public class ColorfulSmokesEffect : ChaosEffect
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

        GetChaosPlugin().RegisterListener(SmokeListener);
    }
    
    public static string UId => "ColorfulSmokes";
    public override string GetEffectName => "Colorful Smokes";
    public override string GetEffectDescription => "Makes smokes colorful!";

    public override void EndEffect()
    {
        if(GetChaosPlugin() == null) return;
        Console.WriteLine("Colorful Smokes Ended");
        GetChaosPlugin().RemoveListener(SmokeListener);
    }

    public override ChaosEffectDuration Duration => ChaosEffectDuration.Long;
}