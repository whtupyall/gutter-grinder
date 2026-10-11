using UnityEngine;

/// <summary>
/// 3 zones, same runner, different mult + look. Original take on portals/dimensions.
/// 0 Scrapyard x1, 1 Rooftops x3, 2 Sewers x8.
/// </summary>
public class ZoneSystem : MonoBehaviour
{
    public static ZoneSystem Instance;
    public int currentZone = 0;

    // Unlock rules (tunable)
    public int zone1NeedKills = 50;
    public int zone1NeedDagger = 15;
    public int zone2NeedKills = 200;
    public int zone2NeedDagger = 30;

    readonly string[] names = { "Scrapyard", "Rooftops", "Sewers" };
    readonly float[] mults = { 1f, 3f, 8f };
    readonly Color[] bgColors = {
        new Color(0.12f, 0.12f, 0.14f),
        new Color(0.08f, 0.12f, 0.25f),
        new Color(0.07f, 0.20f, 0.14f)
    };

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentZone = PlayerPrefs.GetInt("gg_zone", 0);
        currentZone = Mathf.Clamp(currentZone, 0, 2);
    }

    public string ZoneName() => names[currentZone];
    public float ZoneMult() => mults[currentZone];

    public bool CanUnlock(int zone)
    {
        if (zone <= currentZone) return false;
        int kills = GameManager.Instance != null ? GameManager.Instance.totalKills : 0;
        int dagger = WeaponSystem.Instance != null ? WeaponSystem.Instance.daggerLevel : 0;
        if (zone == 1) return kills >= zone1NeedKills && dagger >= zone1NeedDagger;
        if (zone == 2) return kills >= zone2NeedKills && dagger >= zone2NeedDagger;
        return false;
    }

    public bool IsUnlocked(int zone)
    {
        if (zone == 0) return true;
        int kills = GameManager.Instance != null ? GameManager.Instance.totalKills : 0;
        int dagger = WeaponSystem.Instance != null ? WeaponSystem.Instance.daggerLevel : 0;
        if (zone == 1) return kills >= zone1NeedKills && dagger >= zone1NeedDagger;
        if (zone == 2) return kills >= zone2NeedKills && dagger >= zone2NeedDagger;
        return false;
    }

    public bool SwitchTo(int zone)
    {
        if (zone < 0 || zone > 2) return false;
        if (!IsUnlocked(zone)) return false;
        currentZone = zone;
        PlayerPrefs.SetInt("gg_zone", currentZone);
        PlayerPrefs.Save();
        ApplyLook();
        return true;
    }

    public void ApplyLook()
    {
        if (Camera.main != null) Camera.main.backgroundColor = bgColors[currentZone];
    }

    void Start() { ApplyLook(); }

    public void ResetToZero()
    {
        currentZone = 0;
        PlayerPrefs.SetInt("gg_zone", 0);
        ApplyLook();
    }

    // Tougher monsters in later zones: bonus HP
    public int BonusHP() => currentZone; // +0 / +1 / +2
}
