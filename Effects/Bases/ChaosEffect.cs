namespace ChaosPlugin.Effects.Bases;

public abstract class ChaosEffect
{
    public ChaosPlugin? GetChaosPlugin()
    {
        return ChaosPlugin.Plugin;
    }
    public ChaosEffect()
    {
        TimeLeft = GetEffectDuration();
    }
    
    public enum ChaosEffectDuration
    {
        Instantaneous,
        Short,
        Medium,
        Long
    }
    public virtual ChaosEffectDuration Duration => ChaosEffectDuration.Instantaneous;
    public virtual float GetEffectDuration()
    {
        return Duration switch
        {
            ChaosEffectDuration.Instantaneous => 0.0f,
            ChaosEffectDuration.Short => 5f,
            ChaosEffectDuration.Medium => 10f,
            ChaosEffectDuration.Long => 20f,
            _ => 0f
        };
    }
    
    public virtual void StartEffect(){}
    public virtual void EndEffect(){}
    public virtual void TickEffect(float Dt){}

    public string UId = "NONE";
   
    public virtual string GetEffectName => "Name_None";
    public virtual string GetEffectDescription => "Description_None";

    public virtual HashSet<string> IncompatibleEffects => [];

    public float TimeLeft;
    
    public bool IsLoaded = false;
}

