using UnityEngine;

/// <summary>
/// Infinite weapon: Rusty Dagger.
/// cost = baseCost * growth ^ level. Each level +1 dmg, +1 cps.
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance;

    [Header("Dagger (infinite)")]
    public int daggerLevel = 0;
    public double baseCost = 50;
    public double growth = 1.35;
    public int dmgPerLevel = 1;
    public double cpsPerLevel = 1.0;

    [Header("Speed")]
    public float attackInterval = 0.4f; // seconds per hit

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        daggerLevel = PlayerPrefs.GetInt("gg_dagger", 0);
    }

    public double GetCost() => baseCost * System.Math.Pow(growth, daggerLevel);
    public double GetCostForLevel(int level) => baseCost * System.Math.Pow(growth, level);

    public int GetDamage() => 1 + daggerLevel * dmgPerLevel;
    public double GetTotalCPS() => daggerLevel * cpsPerLevel;

    public bool BuyOne()
    {
        double cost = GetCost();
        if (GameManager.Instance == null || !GameManager.Instance.SpendCoins(cost)) return false;
        daggerLevel++;
        GameManager.Instance.Save();
        return true;
    }

    public int BuyMax()
    {
        int bought = 0;
        // cap iterations so a huge wallet can't freeze
        for (int i = 0; i < 1000; i++)
        {
            if (!BuyOne()) break;
            bought++;
        }
        return bought;
    }
}
