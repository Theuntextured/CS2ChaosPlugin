using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class AutoBhop : ChaosEffect
{
    public override ChaosEffectDuration Duration => ChaosEffectDuration.Medium;
    public override void StartEffect()
    {
        Server.ExecuteCommand("sv_enablebunnyhopping 1;" +
                              "sv_maxvelocity 7000;" +
                              "sv_staminamax 0;" +
                              "sv_staminalandcost 0;" +
                              "sv_staminajumpcost 0;" +
                              "sv_accelerate_use_weapon_speed 0;" +
                              "sv_staminarecoveryrate 0;" +
                              "sv_autobunnyhopping 1;" +
                              "sv_airaccelerate 2000");
    }

    public override void EndEffect()
    {
        Server.ExecuteCommand("sv_enablebunnyhopping 0;" +
                              "sv_maxvelocity 3500;" +
                              "sv_staminamax 80;" +
                              "sv_staminalandcost 0.05;" +
                              "sv_staminajumpcost 0.08;" +
                              "sv_accelerate_use_weapon_speed 1;" +
                              "sv_staminarecoveryrate 60;" +
                              "sv_autobunnyhopping 0;" +
                              "sv_airaccelerate 12");
    }

    public override string GetEffectName => "Auto Bhop";
    public override string GetEffectDescription => "Auto Bhop. And fast.";
}