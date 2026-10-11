using UnityEngine;
using System.Collections.Generic;

/// <summary>Core Parts test system: 3 placeholder parts, infinite levels, evo x2 every 100. Values placeholder.</summary>
[System.Serializable]
public class CorePart
{
    public string id = "fists";
    public string baseName = "Rusty Fists";
    public double baseCost = 50;
    public double growth = 1.35;
    public double cpsPerLevel = 1.0;
    public int level = 0;

    static readonly string[] TIER_NAMES = { "Rusty", "Bronze", "Iron", "Steel", "Chrome", "Quantum" };

    public string DisplayName()
    {
        int tier = Mathf.Clamp(level / 100, 0, TIER_NAMES.Length - 1);
        string rest = baseName.Contains(" ") ? baseName.Substring(baseName.IndexOf(" ") + 1) : baseName;
        return $"{TIER_NAMES[tier]} {rest}";
    }

    public double EvoMult() => System.Math.Pow(2.0, level / 100); // placeholder x2 per 100
    public double GetCost() => baseCost * System.Math.Pow(growth, level);
    public double GetCPS() => level * cpsPerLevel * EvoMult();
}
