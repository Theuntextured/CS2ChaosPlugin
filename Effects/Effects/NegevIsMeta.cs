using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ChaosPlugin.Effects.Effects;

public class NegevIsMeta : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
           ChaosUtilities.SetPlayerLoadout(Player, ["weapon_knife", "weapon_negev"]);
        }
    }

    public override string GetEffectName => "Negev is Meta";
    public override string GetEffectDescription => "Give everyone a Negev.";
}