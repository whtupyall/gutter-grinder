using UnityEngine;

/// <summary>
/// Core Parts (CPS ONLY): Rusty Core etc. Replaces Dagger.
/// cost = baseCost * growth ^ level. Each level +CPS, NEVER damage.
/// Hit damage = 1 base until Upgrade-tab Damage upgrades exist (see sheet COMBAT DAMAGE MODEL).
/// </summary>
public class WeaponSystem : MonoBehaviour // kept name to avoid breaking scene refs; displays as Core Parts
{
    public static WeaponSystem Instance;

    [Header("Core (infinite, CPS only)")]
    public int coreLevel = 0;
    public double baseCost = 50;
    public double growth = 1.35;
    public double cpsPerLevel = 1.0;

    [System.Obsolete("Renamed to coreLevel")]
    public int daggerLevel { get => coreLevel; set => coreLevel = value; }

    [Header("Speed")]
    public float attackInterval = 0.4f; // seconds per hit

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // migrate old save key
        int old = PlayerPrefs.GetInt("gg_dagger", 0);
        int cur = PlayerPrefs.GetInt("gg_core", old);
        coreLevel = cur;
    }

    public double GetCost() => baseCost * System.Math.Pow(growth, coreLevel);
    public double GetCostForLevel(int level) => baseCost * System.Math.Pow(growth, level);

    public int GetDamage() => 1; // base only; Damage upgrades add via separate system next
    public double GetTotalCPS() => coreLevel * cpsPerLevel;

    public bool BuyOne()
    {
        double cost = GetCost();
        if (GameManager.Instance == null || !GameManager.Instance.SpendCoins(cost)) return false;
        coreLevel++;
        GameManager.Instance.Save();
        return true;
    }

    public int BuyMax()
    {
        int bought = 0;
        for (int i = 0; i < 1000; i++)
        {
            if (!BuyOne()) break;
            bought++;
        }
        return bought;
    }
}
