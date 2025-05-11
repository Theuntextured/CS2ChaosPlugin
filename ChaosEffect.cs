namespace ChaosPlugin;

public enum ChaosEffectDuration
{
    Instantaneous,
    Short,
    Medium,
    Long
}

/*
    TO OVERRIDE:
    ChaosEffectDuration Duration
    
    public void StartEffect(){}
    public void EndEffect(){} (OPTIONAL)
    public void TickEffect(float Dt){} (OPTIONAL)
    
    public static string UId = "ChaosEffectNone";
    public virtual string GetEffectName => "Name_None";
    public virtual string GetEffectDescription => "Description_None";
    
    static constructor to add to public static Dictionary<string, Type> EffectClasses = [];
 */
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

    public virtual ChaosEffectDuration Duration => ChaosEffectDuration.Instantaneous;
    
    public virtual void StartEffect(){}
    public virtual void EndEffect(){}
    public virtual void TickEffect(float Dt){}

    public static string UId = "ChaosEffectNone";
    public virtual string GetEffectName => "Name_None";
    public virtual string GetEffectDescription => "Description_None";

    public float TimeLeft;
    
    public bool IsLoaded = false;
}

