using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TMP_Text coinsText, cpsText, distText, killsText, questText;

    void Start()
    {
        if (coinsText == null) coinsText = FindText("CoinsText");
        if (cpsText == null) cpsText = FindText("CPSText");
        if (distText == null) distText = FindText("DistText");
        if (killsText == null) killsText = FindText("KillsText");
        if (questText == null) questText = FindText("QuestText");
    }

    TMP_Text FindText(string n)
    {
        GameObject go = GameObject.Find(n);
        return go != null ? go.GetComponent<TMP_Text>() : null;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (coinsText != null) coinsText.text = $"Coins: {Format(GameManager.Instance.coins)} (x{GameManager.Instance.GetCoinMultiplier():F1})";
        if (cpsText != null && WeaponSystem.Instance != null)
        {
            double eff = WeaponSystem.Instance.GetTotalCPS() * GameManager.Instance.GetCoinMultiplier();
            cpsText.text = $"CPS: {Format(eff)}  DMG: {WeaponSystem.Instance.GetDamage()}";
        }
        if (distText != null) distText.text = $"Dist: {(int)GameManager.Instance.distance}m  Best: {(int)GameManager.Instance.bestDistance}m";
        if (killsText != null) killsText.text = $"Kills: {GameManager.Instance.totalKills}";
        if (questText != null && QuestSystem.Instance != null)
        {
            if (!QuestSystem.Instance.shroomUnlocked)
                questText.text = $"Quest: Kill Grubs {QuestSystem.Instance.grubKills}/10 -> unlock Shroom";
            else if (ZoneSystem.Instance != null && ZoneSystem.Instance.currentZone == 0)
                questText.text = $"Unlock Rooftops: {GameManager.Instance.totalKills}/50 kills + Core {WeaponSystem.Instance.coreLevel}/15";
            else
                questText.text = $"Shrooms: {QuestSystem.Instance.shroomKills} Batics: {QuestSystem.Instance.baticKills}";
        }
    }

    public static string Format(double v)
    {
        if (v < 1000) return ((int)v).ToString();
        if (v < 1e6) return (v / 1e3).ToString("F1") + "K";
        if (v < 1e9) return (v / 1e6).ToString("F1") + "M";
        if (v < 1e12) return (v / 1e9).ToString("F1") + "B";
        return (v / 1e12).ToString("F1") + "T";
    }
}
