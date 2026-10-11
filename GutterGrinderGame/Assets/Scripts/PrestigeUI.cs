using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrestigeUI : MonoBehaviour
{
    public TMP_Text prestigeText;
    public Button rebootButton;

    void Start()
    {
        if (prestigeText == null) { var go = GameObject.Find("PrestigeText"); if (go != null) prestigeText = go.GetComponent<TMP_Text>(); }
        if (rebootButton == null) { var go = GameObject.Find("RebootButton"); if (go != null) rebootButton = go.GetComponent<Button>(); }
        if (rebootButton != null) rebootButton.onClick.AddListener(() => PrestigeSystem.Instance?.Reboot());
    }

    void Update()
    {
        if (PrestigeSystem.Instance == null) return;
        int earn = PrestigeSystem.Instance.Earnable();
        if (prestigeText != null)
            prestigeText.text = $"Core Chips: {PrestigeSystem.Instance.coreChips} (+{PrestigeSystem.Instance.BonusMult() * 100 - 100:F0}% CPS)\nLifetime: {HUD.Format(PrestigeSystem.Instance.lifetimeThisReboot)} -> +{earn} on Reboot";
        if (rebootButton != null) rebootButton.interactable = earn > 0;
    }
}
