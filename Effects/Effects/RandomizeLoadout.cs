using ChaosPlugin.Effects.Bases;
using CounterStrikeSharp.API;

namespace ChaosPlugin.Effects.Effects;

public class RandomizeLoadout : ChaosEffect
{
    static string[] Pistols =
    [
        "weapon_deagle",
        "weapon_revolver",
        "weapon_glock",
        "weapon_usp_silencer",
        "weapon_cz75a",
        "weapon_fiveseven",
        "weapon_p250",
        "weapon_tec9",
        "weapon_elite",
        "weapon_hkp2000"
    ];

    private static string[] Primaries =
    [
        "weapon_mp9",
        "weapon_mac10",
        "weapon_bizon",
        "weapon_mp7",
        "weapon_ump45",
        "weapon_p90",
        "weapon_mp5sd",
        "weapon_famas",
        "weapon_galilar",
        "weapon_m4a4",
        "weapon_m4a1_silencer",
        "weapon_ak47",
        "weapon_aug",
        "weapon_sg553",
        "weapon_ssg08",
        "weapon_awp",
        "weapon_scar20",
        "weapon_g3sg1",
        "weapon_nova",
        "weapon_xm1014",
        "weapon_mag7",
        "weapon_sawedoff",
        "weapon_m249",
        "weapon_negev"
    ];

    private static string[] Equipment =
    [
        "item_assaultsuit",
        "item_defuser",
        "weapon_taser",
        "item_kevlar",
        
        "weapon_decoy",
        "weapon_flashbang",
        "weapon_smokegrenade",
        "weapon_hegrenade",
        "weapon_molotov",
        "weapon_incgrenade"
    ];

    public override void StartEffect()
    {
        foreach (var Player in Utilities.GetPlayers())
        {
            if(!Player.IsValid) continue;

            List<string> WeaponsToGive = new List<string>();
            WeaponsToGive.Add("weapon_knife");
            int WeaponIndex = Rand.Next(Pistols.Length + 1);
            if(WeaponIndex < Pistols.Length)
                WeaponsToGive.Add(Pistols[WeaponIndex]);
            
            WeaponIndex = Rand.Next(Primaries.Length + 1);
            if(WeaponIndex < Primaries.Length)
                WeaponsToGive.Add(Primaries[WeaponIndex]);

            foreach (var Item in Equipment)
            {
                if(Rand.NextDouble() > 0.6) // 40% chance to get each item
                    WeaponsToGive.Add(Item);
            }
            
            Player.SetPlayerLoadout(WeaponsToGive.ToArray());
        }
    }

    public override string GetEffectName => "Randomize Loadout";
    public override string GetEffectDescription => "Completely randomizes everybody's loadout.";
}