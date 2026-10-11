using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZoneUI : MonoBehaviour
{
    public TMP_Text zoneText;
    public Button zone0Btn, zone1Btn, zone2Btn;

    void Start()
    {
        if (zoneText == null) { var go = GameObject.Find("ZoneText"); if (go != null) zoneText = go.GetComponent<TMP_Text>(); }
        if (zone0Btn == null) { var go = GameObject.Find("Zone0Button"); if (go != null) zone0Btn = go.GetComponent<Button>(); }
        if (zone1Btn == null) { var go = GameObject.Find("Zone1Button"); if (go != null) zone1Btn = go.GetComponent<Button>(); }
        if (zone2Btn == null) { var go = GameObject.Find("Zone2Button"); if (go != null) zone2Btn = go.GetComponent<Button>(); }
        if (zone0Btn != null) zone0Btn.onClick.AddListener(() => ZoneSystem.Instance?.SwitchTo(0));
        if (zone1Btn != null) zone1Btn.onClick.AddListener(() => ZoneSystem.Instance?.SwitchTo(1));
        if (zone2Btn != null) zone2Btn.onClick.AddListener(() => ZoneSystem.Instance?.SwitchTo(2));
    }

    void Update()
    {
        if (ZoneSystem.Instance == null) return;
        if (zoneText != null)
            zoneText.text = $"Zone: {ZoneSystem.Instance.ZoneName()} x{ZoneSystem.Instance.ZoneMult()}";
        if (zone1Btn != null) zone1Btn.interactable = ZoneSystem.Instance.IsUnlocked(1);
        if (zone2Btn != null) zone2Btn.interactable = ZoneSystem.Instance.IsUnlocked(2);
    }
}
