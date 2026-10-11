using UnityEngine;

/// <summary>
/// Reboot prestige: reset run for Core Chips (+10% CPS each, permanent).
/// Original take on ascension/souls. earnable = floor(sqrt(lifetimeThisReboot / 20000)).
/// </summary>
public class PrestigeSystem : MonoBehaviour
{
    public static PrestigeSystem Instance;
    public int coreChips = 0;
    public double lifetimeThisReboot = 0;
    public double rebootCost = 20000; // coins lifetime needed before first chip

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        coreChips = PlayerPrefs.GetInt("gg_chips", 0);
        double.TryParse(PlayerPrefs.GetString("gg_life", "0"), out lifetimeThisReboot);
    }

    public float BonusMult() => 1f + coreChips * 0.10f;

    public int Earnable()
    {
        if (lifetimeThisReboot < rebootCost) return 0;
        return Mathf.FloorToInt(Mathf.Sqrt((float)(lifetimeThisReboot / rebootCost)));
    }

    public void TrackEarned(double amount)
    {
        lifetimeThisReboot += amount;
    }

    public bool Reboot()
    {
        int gain = Earnable();
        if (gain <= 0) return false;
        coreChips += gain;
        lifetimeThisReboot = 0;

        // reset run
        if (GameManager.Instance != null)
        {
            GameManager.Instance.coins = 0;
            GameManager.Instance.totalKills = 0;
            GameManager.Instance.distance = 0;
        }
        if (WeaponSystem.Instance != null) WeaponSystem.Instance.coreLevel = 0;
        if (QuestSystem.Instance != null) QuestSystem.Instance.ResetKills();
        if (ZoneSystem.Instance != null) ZoneSystem.Instance.ResetToZero();

        Save();
        if (GameManager.Instance != null) GameManager.Instance.Save();
        return true;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("gg_chips", coreChips);
        PlayerPrefs.SetString("gg_life", lifetimeThisReboot.ToString("R"));
        PlayerPrefs.Save();
    }
}
