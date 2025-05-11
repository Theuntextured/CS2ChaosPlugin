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

    public override void Load(bool hotReload)
    {
        Plugin = this;
        RegisterAttributeHandlers(Manager);
        Manager.Load();
        RegisterListener<Listeners.OnTick>(() => { Manager.Tick(); });



        Console.WriteLine("Chaos Plugin loaded.");
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

        string TargetEffect = Command.GetArg(0);
        if (!Manager.EffectClasses.TryGetValue(TargetEffect, out var TargetType)) return;
        Manager.CreateEffect(TargetType);
    }
}

public class ChaosPluginServiceCollection : IPluginServiceCollection<ChaosPlugin>
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        //serviceCollection.AddScoped<ExampleInjectedClass>();
        //serviceCollection.AddLogging(builder => ...);
    }
}
