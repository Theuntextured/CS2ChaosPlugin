using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Cvars;

namespace ChaosPlugin.Effects.Effects;

public class RichKid : ChaosEffect
{
    public override void StartEffect()
    {
        int MaxMoney = ConVar.Find("mp_maxmoney")!.GetPrimitiveValue<int>(); 
        foreach (var Player in Utilities.GetPlayers())
        {
            if (Player.InGameMoneyServices != null) Player.InGameMoneyServices.Account = MaxMoney;
            Utilities.SetStateChanged(Player, "CCSPlayerController", "m_pInGameMoneyServices");
        }
    }

    public override string GetEffectName => "Rich Kid";
    public override string GetEffectDescription => "Gives max money.";
}