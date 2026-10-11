using UnityEngine;

/// <summary>
/// Central bank + distance + save. No death, no fail.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Run")]
    public float runSpeed = 5f;
    public float distance;
    public float bestDistance;

    [Header("Wallet (idle numbers)")]
    public double coins;
    public int totalKills;

    float cpsTimer;
    float saveTimer;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    void Update()
    {
        distance += runSpeed * Time.deltaTime;

        // Passive CPS tick (from weapons) x zone x prestige
        double cps = 0;
        if (WeaponSystem.Instance != null) cps = WeaponSystem.Instance.GetTotalCPS();
        cps *= GetCoinMultiplier();
        if (cps > 0)
        {
            cpsTimer += Time.deltaTime;
            if (cpsTimer >= 1f)
            {
                double gain = cps * cpsTimer;
                coins += gain;
                if (PrestigeSystem.Instance != null) PrestigeSystem.Instance.TrackEarned(gain);
                cpsTimer = 0f;
            }
        }

        if (distance > bestDistance) bestDistance = distance;

        saveTimer += Time.deltaTime;
        if (saveTimer > 5f) { saveTimer = 0f; Save(); }
    }

    public float GetCoinMultiplier()
    {
        float m = 1f;
        if (ZoneSystem.Instance != null) m *= ZoneSystem.Instance.ZoneMult();
        if (PrestigeSystem.Instance != null) m *= PrestigeSystem.Instance.BonusMult();
        return m;
    }

    public void AddCoins(double baseAmount)
    {
        double gain = baseAmount * GetCoinMultiplier();
        coins += gain;
        if (PrestigeSystem.Instance != null) PrestigeSystem.Instance.TrackEarned(gain);
    }
    public bool SpendCoins(double amount)
    {
        if (coins < amount) return false;
        coins -= amount;
        return true;
    }

    public void AddKill(string monsterId)
    {
        totalKills++;
        if (QuestSystem.Instance != null) QuestSystem.Instance.RegisterKill(monsterId);
    }

    public void Save()
    {
        PlayerPrefs.SetString("gg_coins", coins.ToString("R"));
        PlayerPrefs.SetFloat("gg_best", bestDistance);
        PlayerPrefs.SetInt("gg_kills", totalKills);
        if (WeaponSystem.Instance != null) PlayerPrefs.SetInt("gg_dagger", WeaponSystem.Instance.daggerLevel);
        if (QuestSystem.Instance != null)
        {
            PlayerPrefs.SetInt("gg_grub", QuestSystem.Instance.grubKills);
            PlayerPrefs.SetInt("gg_shroom", QuestSystem.Instance.shroomKills);
            PlayerPrefs.SetInt("gg_batic", QuestSystem.Instance.baticKills);
        }
        if (PrestigeSystem.Instance != null) PrestigeSystem.Instance.Save();
        PlayerPrefs.Save();
    }

    void Load()
    {
        double c = 0;
        double.TryParse(PlayerPrefs.GetString("gg_coins", "0"), out c);
        coins = c;
        bestDistance = PlayerPrefs.GetFloat("gg_best", 0);
        totalKills = PlayerPrefs.GetInt("gg_kills", 0);
    }

    void OnApplicationPause(bool pause) { if (pause) Save(); }
    void OnApplicationQuit() { Save(); }

    [ContextMenu("Reset Save")]
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        coins = 0; totalKills = 0; distance = 0; bestDistance = 0;
        if (WeaponSystem.Instance != null) WeaponSystem.Instance.daggerLevel = 0;
    }
}
