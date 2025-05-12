using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class London : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            //Player.RemoveWeapons();
           // Player.GiveNamedItem("weapon_knife");
           ChaosUtilities.RemoveWeaponsExceptKnife(Player);
        }
        
        foreach (var entity in Utilities.GetAllEntities())
        {
            if (!entity.IsValid)
                continue;

            if (entity.DesignerName.StartsWith("weapon_"))
            {
                entity.Remove();
            }
        }
        
        
    }

    public override string GetEffectName => "London";
    public override string GetEffectDescription => "Knife only. Go stab each other.";
}