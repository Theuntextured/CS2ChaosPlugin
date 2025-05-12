using ChaosPlugin.Effects.Bases;

namespace ChaosPlugin.Effects.Effects;

public class Nothing : ChaosEffect
{
    public static string StaticUId => "Nothing";
    public override string GetEffectName => "Nothing";
    public override string GetEffectDescription => "Does absolutely nothing.";
}