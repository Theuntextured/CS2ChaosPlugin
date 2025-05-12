using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class GoEco : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            if (Player.InGameMoneyServices != null) Player.InGameMoneyServices.Account = 0;
            Utilities.SetStateChanged(Player, "CCSPlayerController", "m_pInGameMoneyServices");
        }
    }

    public override string GetEffectName => "Go Eco";
    public override string GetEffectDescription => "Removes all money.";
}