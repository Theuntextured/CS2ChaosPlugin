using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using ChaosPlugin.Effects.Bases;
using ChaosPlugin.Effects.Effects;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Events;


namespace ChaosPlugin;

public class ChaosManager
{
    public Random Rand = new Random();

    public Dictionary<string, Type> EffectClasses = [];

    public List<ChaosEffect> CurrentEffects = [];

    public static string RepeatString(string Str, int Amount)
    {
        return string.Concat(Enumerable.Repeat(Str, Amount));
    }
    
    private void RegisterEffectClasses()
    {
        // Get all types in the current assembly
        var EffectTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ChaosEffect)) && !t.IsAbstract)
            .ToList();
        
        Console.WriteLine($"{EffectTypes.Count} effects found.");
        foreach (var Type in EffectTypes)
        {
            Console.WriteLine($"Registering {Type.FullName}");
            // Assuming each class has a static UId property
            {
                string UId = Type.Name;
                if (EffectClasses.TryAdd(UId, Type))
                {
                    Console.WriteLine($"{UId} has been registered.");
                }
            }
        }
    }

    public void Load()
    {
        //EffectClasses.Add("colorful_smoke", typeof(ColorfulSmokesEffect));
        RegisterEffectClasses();
        ChaosPlugin.Plugin?.RegisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    public void EmitSoundToAll(string Sound)
    {
        var Players = Utilities.GetPlayers();
        foreach (var Player in Players)
        {
            Player.ExecuteClientCommand("play " + Sound);
        }
    }

    string MakeProgressBar(float Percent, int BarLength)
    {
        const string BarFill = "\u2593";
        const string BarEmpty = "\u2591";

        var BarFillCount = (int)Math.Round(Percent * BarLength);
        return RepeatString(BarFill, BarFillCount) + RepeatString(BarEmpty, BarLength - BarFillCount);
    }

    public void Tick()
    {
        var GameRulesEntity = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("cs_gamerules").SingleOrDefault();
        if (GameRulesEntity == null) return;
        var GameRules = GameRulesEntity.As<CCSGameRulesProxy>().GameRules;
        if (GameRules == null) return;
        if (!GameRules.HasMatchStarted)
        {
            RemoveAllEffects();
            return;
        }
        const float Dt = 0.015625f; // 1/64

        for (var i = 1; i <= 3; i++)
        {
            if(CurrentTime + Dt >= (TimePerEffect - i) && CurrentTime < (TimePerEffect - i))
            {
                EmitSoundToAll("\\sounds\\ui\\buttonrollover.vsnd_c");
                break;
            }
        }

        CurrentTime += Dt;
        if (CurrentTime >= TimePerEffect)
        {
            CurrentTime -= TimePerEffect;
            CreateEffect(EffectClasses.ElementAt(Rand.Next(EffectClasses.Count)).Key);
            EmitSoundToAll("\\sounds\\ui\\buttonclick.vsnd_c");
        }
        
        for (int i = 0; i < CurrentEffects.Count; ++i)
        {
            var Effect = CurrentEffects[i];
            Effect.TimeLeft -= Dt;

            if (Effect.TimeLeft <= 0)
            {
                RemoveEffect(i);
                i--;
                continue;
            }
            Effect.TickEffect(Dt);
        }

        float TimePercent = CurrentTime / TimePerEffect;
        string Color = "green";
        if (TimePercent > 0.5f) 
            Color = TimePercent > 0.8f ? "red" : "orange";
        
        string BarText = $"<font color='{Color}'>Next effect:<br>{MakeProgressBar(TimePercent, 10)}</font>";

        string EffectsText = "<br>{default}";
        
        foreach (var Effect in CurrentEffects)
        {
            EffectsText +=
                $"{Effect.GetEffectName} {MakeProgressBar(Effect.TimeLeft / Effect.GetEffectDuration(), 8)}<br>";
        }
        
        foreach (var Player in Utilities.GetPlayers())
        {
            Player.PrintToCenterHtml(BarText + GetColoredText(EffectsText));
        }
    }

    public void RemoveAllEffects()
    {
        while (CurrentEffects.Count > 0) 
            RemoveEffect(CurrentEffects.Count - 1);
    }

    public void Unload()
    {
        RemoveAllEffects();
        ChaosPlugin.Plugin?.DeregisterEventHandler<EventRoundStart>(OnRoundStart);
    }
    
    public ChaosEffect? CreateEffect(string Effect)
    {
        if (ChaosPlugin.Plugin == null) return null;
        
        Console.WriteLine($"Adding effect {Effect}");
        
        if (!EffectClasses.TryGetValue(Effect, out var EffectType))
        {
            Console.WriteLine($"{Effect} could not be created because it does not exist.");
            return null;
        }

        foreach (var CurrentEffect in CurrentEffects)
        {
            if (CurrentEffect.UId == Effect)
            {
                CurrentEffect.TimeLeft = CurrentEffect.GetEffectDuration();
                CurrentEffects.Remove(CurrentEffect);
                CurrentEffects.Add(CurrentEffect);
                return CurrentEffect;
            }
        }
        
        ChaosEffect? NewEffect = (ChaosEffect?)Activator.CreateInstance(EffectType);
        if (NewEffect == null)
        {
            Console.WriteLine($"{Effect} could not be created.");
            return null;
        }

        if(NewEffect.IncompatibleEffects.Count > 0 || NewEffect.Category != null) {
            for (int i = 0; i < CurrentEffects.Count; ++i)
            {
                var CurrentEffect = CurrentEffects[i];
                if (NewEffect.IncompatibleEffects.Contains(CurrentEffect.UId) ||
                    (NewEffect.Category == CurrentEffect.Category && NewEffect.Category != null))
                {
                    RemoveEffect(i);
                    i--;
                }
            }
        }
        
        if(NewEffect.TimeLeft > 0) CurrentEffects.Add(NewEffect);
        NewEffect.IsLoaded = true;
        NewEffect.UId = Effect;
        ChaosPlugin.Plugin.RegisterAttributeHandlers(NewEffect);
        NewEffect.StartEffect();
        if(NewEffect.TimeLeft == 0) NewEffect.IsLoaded = false;
        
        var Players = Utilities.GetPlayers();
        Server.PrintToChatAll(GetColoredText("{red}New Effect: " + NewEffect.GetEffectName));
        Server.PrintToChatAll(GetColoredText("{green}" + NewEffect.GetEffectDescription));
        
        return NewEffect;
    }
    
    public static string GetColoredText(string Message)
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

    private void RemoveEffect(int Pos)
    {
        Debug.Assert(Pos >=0 && Pos < CurrentEffects.Count);
        CurrentEffects[Pos].EndEffect();
        Console.WriteLine($"{CurrentEffects[Pos].GetEffectName} has been removed.");
        CurrentEffects[Pos].IsLoaded = false;
        CurrentEffects.RemoveAt(Pos);
    }
    
    [GameEventHandler]
    public HookResult OnPlayerChat(EventPlayerChat @event, GameEventInfo info)
    {
        var SplittedText = @event.Text.Split(" ");
        if(ChaosPlugin.Plugin == null) return HookResult.Continue;
        if (SplittedText[0].ToLower() == ".chaos")
        {
            var Player = Utilities.GetPlayerFromUserid(@event.Userid);
            if (Player != null)
                Player.PrintToCenterAlert(ChaosPlugin.Plugin.ReadyText);
            return HookResult.Continue;
        }

        if (SplittedText[0].ToLower() == ".chaos_effect" && SplittedText.Length > 1)
        {
            Console.WriteLine($"Trying to add effect \"{SplittedText[1]}\".");
            CreateEffect(SplittedText[1]);
            return HookResult.Continue;
        }
        
        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        ExplodeInMidair.GraceTime = 0.5f;
        
        return HookResult.Continue;
    }

    public float CurrentTime = 0.0f;
    public float TimePerEffect = 5;
}