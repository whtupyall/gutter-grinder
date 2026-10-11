using UnityEngine;

/// <summary>Kill quests gate new monsters + zones.</summary>
public class QuestSystem : MonoBehaviour
{
    public static QuestSystem Instance;
    public int grubKills;
    public int shroomKills;
    public int baticKills;
    public bool shroomUnlocked => grubKills >= 10;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        grubKills = PlayerPrefs.GetInt("gg_grub", 0);
        shroomKills = PlayerPrefs.GetInt("gg_shroom", 0);
        baticKills = PlayerPrefs.GetInt("gg_batic", 0);
    }

    public void RegisterKill(string id)
    {
        if (id == "grub") grubKills++;
        else if (id == "shroom") shroomKills++;
        else if (id == "batic") baticKills++;
    }

    public void ResetKills()
    {
        grubKills = 0; shroomKills = 0; baticKills = 0;
    }
}
