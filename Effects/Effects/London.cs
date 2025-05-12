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
        
        foreach (var Entity in Utilities.GetAllEntities())
        {
            if (!Entity.IsValid)
                continue;

            if (Entity.DesignerName.StartsWith("weapon_"))
            {
                Entity.Remove();
            }
        }
        
        
    }

    public override string GetEffectName => "London";
    public override string GetEffectDescription => "Knife only. Go stab each other.";
}