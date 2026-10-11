using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Core Parts holder: 3 test parts, CPS ONLY. Values placeholder, exact tuning later.
/// Keeps old WeaponSystem name + API so monsters/HUD keep working.
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance;

    [Header("Core Parts (test set)")]
    public List<CorePart> parts = new List<CorePart>();

    [Header("Speed")]
    public float attackInterval = 0.4f;

    [System.Obsolete("Use parts[0].level")]
    public int coreLevel { get => parts.Count > 0 ? parts[0].level : 0; set { if (parts.Count > 0) parts[0].level = value; } }
    [System.Obsolete("Renamed to coreLevel")]
    public int daggerLevel { get => coreLevel; set => coreLevel = value; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (parts == null || parts.Count == 0)
        {
            parts = new List<CorePart>
            {
                new CorePart { id = "fists", baseName = "Rusty Fists", baseCost = 50, growth = 1.35, cpsPerLevel = 1.0 },
                new CorePart { id = "arm", baseName = "Rusty Arm", baseCost = 400, growth = 1.4, cpsPerLevel = 6.0 },
                new CorePart { id = "legs", baseName = "Rusty Legs", baseCost = 2500, growth = 1.45, cpsPerLevel = 30.0 },
            };
        }
        Load();
    }

    void Load()
    {
        // migrate very old single-key saves into fists
        int oldDagger = PlayerPrefs.GetInt("gg_dagger", 0);
        int oldCore = PlayerPrefs.GetInt("gg_core", oldDagger);
        for (int i = 0; i < parts.Count; i++)
        {
            int lv = PlayerPrefs.GetInt($"gg_core_{i}", i == 0 ? oldCore : 0);
            parts[i].level = lv;
        }
    }

    public void SaveLevels()
    {
        for (int i = 0; i < parts.Count; i++)
            PlayerPrefs.SetInt($"gg_core_{i}", parts[i].level);
        PlayerPrefs.SetInt("gg_core", parts.Count > 0 ? parts[0].level : 0);
    }

    public double GetTotalCPS()
    {
        double t = 0;
        foreach (var p in parts) t += p.GetCPS();
        return t;
    }

    public int GetDamage() => 1; // base only until Damage upgrades

    public double GetCost() => parts.Count > 0 ? parts[0].GetCost() : 0;
    public double GetCostForLevel(int level) => parts.Count > 0 ? parts[0].baseCost * System.Math.Pow(parts[0].growth, level) : 0;

    public bool BuyOne() => BuyOne(0);
    public int BuyMax() => BuyMax(0);

    public bool BuyOne(int idx)
    {
        if (idx < 0 || idx >= parts.Count) return false;
        var p = parts[idx];
        double cost = p.GetCost();
        if (GameManager.Instance == null || !GameManager.Instance.SpendCoins(cost)) return false;
        p.level++;
        GameManager.Instance.Save();
        return true;
    }

    public int BuyMax(int idx)
    {
        int bought = 0;
        for (int i = 0; i < 1000; i++)
        {
            if (!BuyOne(idx)) break;
            bought++;
        }
        return bought;
    }
}
