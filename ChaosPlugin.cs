using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChaosPlugin;

public class ChaosPlugin : BasePlugin
{
    public override string ModuleName => "Chaos Plugin";
    public override string ModuleVersion => "v0.0.1";
    public string ReadyText => ModuleName + " " + ModuleVersion + " ready.";

    public ChaosManager Manager = new ChaosManager();
    public static ChaosPlugin? Plugin = null;
    
    private CCSGameRules? _gameRules;

    public override void Load(bool hotReload)
    {
        Plugin = this;
        RegisterAttributeHandlers(Manager);
        Manager.Load();
        RegisterListener<Listeners.OnTick>(OnTick);
        RegisterListener<Listeners.OnMapStart>(OnMapStartHandler);

        Console.WriteLine("Chaos Plugin loaded.");
    }
    
    private void OnMapStartHandler(string mapName)
    {
        _gameRules = null;
    }

    private void InitializeGameRules()
    {
        var GameRulesProxy = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault();
        _gameRules = GameRulesProxy?.GameRules;
    }

    private void OnTick()
    {
        if (_gameRules == null)
        {
            InitializeGameRules();
        }
        else
        {
            _gameRules.GameRestart = _gameRules.RestartRoundTime < Server.CurrentTime;
        }
        Manager.Tick();
    }

    public override void Unload(bool hotReload)
    {
        Manager.Unload();
        Plugin = null;
        Console.WriteLine("Chaos Plugin unloaded.");
    }

    [ConsoleCommand("chaos", "Chaos plugin status.")]
    public void OnChaosCommand(CCSPlayerController? Player, CommandInfo Command)
    {
        if (Player == null)
        {
            Console.WriteLine(ReadyText);
            return;
        }

        Player.PrintToConsole(ReadyText);
        Console.WriteLine(ReadyText);
    }

    [ConsoleCommand("chaos_next", "Start the next chaos effect.")]
    public void OnChaosNextCommand(CCSPlayerController? Player, CommandInfo Command)
    {
        Manager.CurrentTime = Manager.TimePerEffect;
    }

    [ConsoleCommand("chaos_effect", "Start the next chaos effect.")]
    public void OnChaosEffectCommand(CCSPlayerController? Player, CommandInfo Command)
    {
        Manager.CurrentTime = 0;
        if (Command.ArgCount < 1)
        {
            Manager.CurrentTime = Manager.TimePerEffect;
            return;
        }

        string TargetEffect = Command.GetArg(1);
        Manager.CreateEffect(TargetEffect);
    }

    [ConsoleCommand("chaos_list", "Lists all chaos effects available.")]
    [ConsoleCommand("chaos_effects", "Lists all chaos effects available.")]
    public void OnChaosListCommand(CCSPlayerController? Player, CommandInfo Command)
    {
        Command.ReplyToCommand("List of chaos effects:");
        foreach (var Effect in Manager.EffectClasses)
        {
            Command.ReplyToCommand(Effect.Key);
        }
    }
}