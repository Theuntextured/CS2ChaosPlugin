using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class ValveDm : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            ChaosUtilities.SetPlayerLoadout(Player, ["weapon_knife", "weapon_awp"]);
        }
    }

    public override string GetEffectName => "Valve DM Experience";
    public override string GetEffectDescription => "Give everyone an AWP.";
}