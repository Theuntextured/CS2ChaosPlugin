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
           Player.SetPlayerLoadout(["weapon_knife", "weapon_negev", "item_assaultsuit"]);
        }
    }

    public override string GetEffectName => "Negev is Meta";
    public override string GetEffectDescription => "Give everyone a Negev.";
}