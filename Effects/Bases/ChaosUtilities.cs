using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Memory;

namespace ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

public static class ChaosUtilities
{
    public static void SpawnExplosion(Vector Origin, CCSPlayerController? Thrower = null, int Magnitude = 100, int Radius = 250)
    {
        CBasePlayerPawn? ThrowerPawn = null;
        if (Thrower != null)
        {
            ThrowerPawn = Thrower.Pawn.Value;
        }
        var Grenade = Utilities.CreateEntityByName<CHEGrenadeProjectile>("hegrenade_projectile");
        if (Grenade == null) return;    
        Grenade.Damage = Magnitude;
        Grenade.DmgRadius = Radius;
        if (ThrowerPawn != null) Grenade.TeamNum = ThrowerPawn.TeamNum;
        Grenade.Teleport(Origin);
        Grenade.DispatchSpawn();
        if (Thrower != null) Grenade.Thrower.Raw = Thrower.Pawn.Raw;
        Grenade.AcceptInput("InitializeSpawnFromWorld", ThrowerPawn, ThrowerPawn);
        Grenade.DetonateTime = 0;
    }
    
    public static void Shuffle<T>(this List<T> List)
    {
        int n = List.Count;
        Random Rng = new Random();
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            (List[k], List[n]) = (List[n], List[k]);
        }
    }

    public static void RemoveWeaponsExceptKnife(this CCSPlayerController Player)
    {
        Player.SetPlayerLoadout(["weapon_knife", "item_assaultsuit"]);
    }

    public static void SetPlayerLoadout(this CCSPlayerController Player, string[] Loadout)
    {
        if (!Player.IsValid || !Player.Pawn.IsValid || !Player.PawnIsAlive)
            return;
        bool HasC4 = false;
        var WeaponServices = Player.Pawn.Value?.WeaponServices;
        if (WeaponServices != null)
        {
            foreach (var WeaponHandle in WeaponServices.MyWeapons)
            {
                if (WeaponHandle.IsValid)
                {
                    var Weapon = WeaponHandle.Value;
                    if (Weapon != null && Weapon.IsValid)
                    {
                        if (Weapon.DesignerName == "weapon_c4")
                        {
                            HasC4 = true;
                        }
                    }
                }
            }
        }
        
        Player.RemoveWeapons();
        Server.NextFrame(() =>
        {
            foreach(var Weapon in Loadout)
                Player.GiveNamedItem(Weapon);
            if (HasC4)
                Player.GiveNamedItem("weapon_c4");
        });
    }

    /// <summary>
    /// Convert HSV to RGB
    /// h is from 0-360
    /// s,v values are 0-1
    /// </summary>
    public static Color HsvToRgb(double H, double S, double V)
    {
        int R, G, B;
        HsvToRgb(H, S, V, out R, out G, out B);
        
        return Color.FromArgb(R, G, B);
    }
    
    /// <summary>
    /// Convert HSV to RGB
    /// h is from 0-360
    /// s,v values are 0-1
    /// r,g,b values are 0-255
    /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
    /// </summary>
    public static void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
    {
      // ######################################################################
      // T. Nathan Mundhenk
      // mundhenk@usc.edu
      // C/C++ Macro HSV to RGB

      double H = h;
      while (H < 0) { H += 360; };
      while (H >= 360) { H -= 360; };
      double R, G, B;
      if (V <= 0)
        { R = G = B = 0; }
      else if (S <= 0)
      {
        R = G = B = V;
      }
      else
      {
        double hf = H / 60.0;
        int i = (int)Math.Floor(hf);
        double f = hf - i;
        double pv = V * (1 - S);
        double qv = V * (1 - S * f);
        double tv = V * (1 - S * (1 - f));
        switch (i)
        {

          // Red is the dominant color

          case 0:
            R = V;
            G = tv;
            B = pv;
            break;

          // Green is the dominant color

          case 1:
            R = qv;
            G = V;
            B = pv;
            break;
          case 2:
            R = pv;
            G = V;
            B = tv;
            break;

          // Blue is the dominant color

          case 3:
            R = pv;
            G = qv;
            B = V;
            break;
          case 4:
            R = tv;
            G = pv;
            B = V;
            break;

          // Red is the dominant color

          case 5:
            R = V;
            G = pv;
            B = qv;
            break;

          // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

          case 6:
            R = V;
            G = tv;
            B = pv;
            break;
          case -1:
            R = V;
            G = pv;
            B = qv;
            break;

          // The color is not defined, we should throw an error.

          default:
            //LFATAL("i Value error in Pixel conversion, Value is %d", i);
            R = G = B = V; // Just pretend its black/white
            break;
        }
      }
      r = Clamp((int)(R * 255.0));
      g = Clamp((int)(G * 255.0));
      b = Clamp((int)(B * 255.0));
    }

    /// <summary>
    /// Clamp a value to 0-255
    /// </summary>
    private static int Clamp(int i)
    {
      if (i < 0) return 0;
      if (i > 255) return 255;
      return i;
    }
    
}