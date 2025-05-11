using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;


namespace ChaosPlugin;
public class ChaosManager
{
    Random Rand = new Random();
    
    public Dictionary<string, Type> EffectClasses = [];

    public List<ChaosEffect> CurrentEffects = [];


    private void AddEffectType(Type effectType)
    {
        // Retrieve the PropertyInfo for the static 'UId' property
        var UidProperty = effectType.GetProperty("UId", BindingFlags.Public | BindingFlags.Static);

        if (UidProperty == null)
            throw new InvalidOperationException($"Type {effectType.FullName} does not have a public static property named 'UId'.");

        // Get the value of the 'UId' property
        var UidValue = UidProperty.GetValue(null);

        if (UidValue == null)
            throw new InvalidOperationException($"The 'UId' property of type {effectType.FullName} returned null.");

        // Add the effect type to the dictionary using the retrieved UId
        EffectClasses.Add((string)UidValue, effectType);
    }
    public void Load()
    {
        AddEffectType(typeof(ColorfulSmokesEffect));
    }

    public void Tick()
    {
        const float Dt = 0.015625f; // 1/64

        CurrentTime += Dt;
        if (CurrentTime >= TimePerEffect)
        {
            CurrentTime -= TimePerEffect;
            CreateEffect(EffectClasses.ElementAt(Rand.Next(EffectClasses.Count)).Value);
        }
        
        for (int i = 0; i < CurrentEffects.Count; ++i)
        {
            var Effect = CurrentEffects[i];
            Effect.TimeLeft -= Dt;

            if (Effect.TimeLeft <= 0)
            {
                RemoveEffect(i);
                continue;
            }
            Effect.TickEffect(Dt);
        }
    }

    public void Unload()
    {
        while (CurrentEffects.Count > 0)
        {
            RemoveEffect(CurrentEffects.Count - 1);
        }
    }

    public ChaosEffect? CreateEffect(Type Effect)
    {
        if (ChaosPlugin.Plugin == null) return null;
        ChaosEffect? NewEffect = (ChaosEffect?)Activator.CreateInstance(Effect);
        if (NewEffect == null) return null;
        if(NewEffect.TimeLeft > 0) CurrentEffects.Add(NewEffect);
        NewEffect.IsLoaded = true;
        ChaosPlugin.Plugin.RegisterAttributeHandlers(NewEffect);
        NewEffect.StartEffect();
        if(NewEffect.TimeLeft == 0) NewEffect.IsLoaded = false;
        
        var Players = Utilities.GetPlayers();
        Server.PrintToChatAll(GetColoredText("{red}New Effect: " + NewEffect.GetEffectName));
        Server.PrintToChatAll(GetColoredText("{green}" + NewEffect.GetEffectDescription));
        
        return NewEffect;
    }
    
    private static string GetColoredText(string Message)
    {
        Dictionary<string, int> ColorMap = new()
        {
            { "{default}", 1 },
            { "{white}", 1 },
            { "{darkred}", 2 },
            { "{purple}", 3},
            { "{green}", 4 },
            { "{lightgreen}", 5 },
            { "{slimegreen}", 6 },
            { "{red}", 7 },
            { "{grey}", 8 },
            { "{yellow}", 9 },
            { "{invisible}", 10 },
            { "{lightblue}", 11 },
            { "{blue}", 12 },
            { "{lightpurple}", 13 },
            { "{pink}", 14 },
            { "{fadedred}", 15 },
            { "{gold}", 16 },
            // No more colors are mapped to CS2
        };

        // Use a regular expression to find and replace color codes
        string Pattern = "{(\\w+)}"; // Matches {word}
        string Replaced = Regex.Replace(Message, Pattern, match =>
        {
            string ColorCode = match.Groups[1].Value;
            if (ColorMap.TryGetValue("{" + ColorCode + "}", out int Replacement))
            {
                // A little hack to get the color code to work
                return Convert.ToChar(Replacement).ToString();
            }
            // If the color code is not found, leave it unchanged
            return match.Value;
        });
        // Non-breaking space - a little hack to get all colors to show
        return $"\u200B{Replaced}";
    }

    void RemoveEffect(int Pos)
    {
        Debug.Assert(Pos >=0 && Pos < CurrentEffects.Count);
        CurrentEffects[Pos].EndEffect();
        CurrentEffects[Pos].IsLoaded = false;
        CurrentEffects.RemoveAt(Pos);
    }
    
    [GameEventHandler]
    public HookResult OnPlayerChat(EventPlayerChat @event, GameEventInfo info)
    {
        if(ChaosPlugin.Plugin == null) return HookResult.Continue;
        if (@event.Text.ToLower() != ".chaos") return HookResult.Continue;
        var Player = Utilities.GetPlayerFromUserid(@event.Userid);
        if(Player != null)
            Player.PrintToCenterAlert(ChaosPlugin.Plugin.ReadyText);

        return HookResult.Continue;
    }

    public float CurrentTime = 0.0f;
    public float TimePerEffect = 20.0f;
}