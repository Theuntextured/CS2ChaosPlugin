# Chaos Plugin for CS2

This is a server-side plugin that adds random fun effects on a time interval in Counter Strike 2. The idea is inspired by the [GTAV Chaos Mod by pongo1231](https://www.gta5-mods.com/scripts/chaos-mod-v-beta).

The plugin is made using C# via [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp).

## Installation

1. Make sure to have [MetaMod:Source](https://www.metamodsource.net/downloads.php/?branch=master) and [CSSharp With Runtime](https://github.com/roflmuffin/CounterStrikeSharp/releases) installed as described [here](https://docs.cssharp.dev/docs/guides/getting-started.html).
2. Download the latest release of [ChaosPlugin](https://github.com/Theuntextured/CS2ChaosPlugin/releases).
3. Extract the contents of the zip file into [CS2 Server Dir]\game\csgo\addons\counterstrikesharp\plugins\
4. Start the server and type `.chaos` in chat or `chaos` in the console to verify that the plugin has loaded correctly.

## Contributing

I accept contributions via new effects. These effects must be created in a new file located in [/Effects/Effects/](https://github.com/Theuntextured/CS2ChaosPlugin/tree/master/Effects/Effects) if it is an effect that should be loaded, or under [/Effects/Bases/](https://github.com/Theuntextured/CS2ChaosPlugin/tree/master/Effects/Bases) if it is an abstract class that should not be spawned in as an effect.

### How does the plugin work?

**ChaosPlugin** - The plugin class. This is the class that deals with initializing the whole plugin, getting plugin information etc. 
It also manages console commands and CVars. It also contains code "borrowed" from poggu to fix flashing center text display.
* This contains a static property called `Plugin`, which is the instance of the plugin. Will be `null` if the plugin is not loaded.
* Another useful property is `Manager` (not static this time), which is simply the instance of the `EffectManager` which is being used.

**ChaosManager** - This is a class that manages all the effects, adds them, removes them and displays relevant information about them.
It also automatically registers all relevant effect types to be used. Here are some useful fields and functions:
* `Random Rand` - An instance of the `Random` class
* `List<ChaosEffect> CurrentEffects` - The list of the currently active effects
* `public static string RepeatString(string Str, int Amount)` - Repeats a string `Str` `Amount` times
* `public void EmitSoundToAll(string Sound)` - Plays a sound from a string to all players.
* `MakeProgressBar(float Percent, int BarLength)` - Uses characters ▓ and ░ to make a string progress bar of length `BarLength` characters.
* `public void RemoveAllEffects()` - Removes all active effects.
* `public ChaosEffect? CreateEffect(string Effect)` - Creates an effect from its UId
* `public static string GetColoredText(string Message)` - Converts a string containing `{[color]} (where [color] is replaced with the name of a color) with a string that can be used to create colored text in CS2. The supported colors are:
  * default
  * white
  * darkred
  * purple
  * green
  * lightgreen
  * slimegreen
  * red
  * grey
  * yellow
  * invisible
  * lightblue
  * blue
  * lightpurple
  * pink
  * fadedred
  * gold


**ChaosEffect** - The base class for all effects. For an effect to be valid, it should have certain methods, fields and properties overridden, which are stated below:

* `public virtual ChaosEffectDuration Duration` - This is a way to automatically handle duration. The way this is used is defined in `GetEffectDuration`. By default, this returns `ChaosEffectDuration.Instantaneous`, meaning that the effect will have no duration.
    * Example: `public override ChaosEffectDuration Duration => ChaosEffectDuration.Long;`
* `public static string StaticUId` - The base class does not contain this field, since it is supposed to be static. It will be used to register the effect. Simply add this property to any effect that should be loaded in by the manager.
  * Example: `public static string StaticUId => "ColorfulSmokes";`
* `public virtual string GetEffectName` - The display name of the effect. 
  * Example: `public override string GetEffectName => "Colorful Smokes";`
* `public virtual string GetEffectDescription` - The description of the effect. This will be displayed in chat when the effect is spawned. It should therefore be quite brief.
  * Example: `public override string GetEffectDescription => "Makes smokes colorful!";`
* OPTIONAL `public virtual void StartEffect()` - The function gets called when the effect is spawned.
* OPTIONAL `public virtual void EndEffect()` - The function gets called when the effect ends or gets interrupted. (Only called when the duration is >0)
* OPTIONAL `public virtual void TickEffect(float Dt)` - The function gets called on tick while the effect is active. (Only called when the duration is >0)
* OPTIONAL `public virtual float GetEffectDuration()` - Should return the duration of the effect. returning 0 means no duration, which has the above consequences.
* OPTIONAL `public virtual HashSet<string> IncompatibleEffects` - Should return a list of incompatible effect UIds which should be removed if active. For example an effect which increases gravity would be incompatible with one that lowers it.

**IMPORTANT NOTE:** If an effect is not to be loaded in by the effect manager as a usable effect, then it should be marked as `abstract`
**NOTE:** If an effect is an event listener, remember to remove it as a listener when the effect ends.
## To-Do

* Effect display

### Effects to do

* ~~ColorfulSmokes~~
* ~~Nothing~~
* Explode in midair
* Shuffle locations
* Call of Duty (no recoil, no spread)
* Moon gravity
* Poor boy (remove all money)
* Max money
* London (Everyone is left with knife only)
* Kill random player
* Everyone is invincible (Except bomb planter and bomb diffuser)
* 0.2x game speed
* 2x game speed
* Explosive combat (Every hit spawns an explosion)
* Back to spawn (Everyone gets teleported to spawn)
* Randomize loadout
* Infinite nades
* Heavy recoil
* Flashbang (Everyone gets flashed)
* CS2 Moment (Extreme spread)
* Pacifist (Killing someone will blow you up - Except for killing bomb planter or diffuser)
* No gravity
* SILENCE! (Push to talk will blow you up)
* Flip everyone (Everyone turns 180 degrees)
* OHKO (1hp, gives back hp on end)
* Invisibility (When sound is not being made)
* Minimal damage
* Valve DM experience (Everyone is given an awp)
* Schizo (Plays misleading sounds)
* LAG (rolls back position frequently)
* Play CSGO intro music
* Unbind W (does not rebind it. Player should do it manually)
* m_yaw (HELICOPTER HELICOPTER)
* Airstrike (Spawn HEs falling from the sky)
* Spawn chicken
* Bomb can be planted anywhere
* Buy anywhere
* Teleport random CT to C4
* spawn smoke
* spawn decoy grenade
* Give away locations (Play "AAH!" sfx at every player's location)
* Randomize health
* Negev is meta (Give everyone a negev)
