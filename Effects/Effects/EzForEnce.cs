using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class EzForEnce : ChaosEffect
{
    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            Player.ExecuteClientCommand("play \\sounds\\music\\theverkkars_01\\roundmvpanthem_01.vsnd_c");
        }
    }

    public override string GetEffectName => "Ez For Ence";
    public override string GetEffectDescription => "Easy for ENCE, ENCE, ENCE Dens putted upperbelt Putted upperbelt";
}