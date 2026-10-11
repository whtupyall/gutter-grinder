using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public TMP_Text coreInfoText;
    public Button buyOneButton;
    public Button buyMaxButton;

    void Start()
    {
        if (coreInfoText == null) { GameObject go = GameObject.Find("CoreText"); if (go == null) go = GameObject.Find("DaggerText"); if (go != null) coreInfoText = go.GetComponent<TMP_Text>(); }
        if (buyOneButton == null) { GameObject go = GameObject.Find("Buy1Button"); if (go != null) buyOneButton = go.GetComponent<Button>(); }
        if (buyMaxButton == null) { GameObject go = GameObject.Find("BuyMaxButton"); if (go != null) buyMaxButton = go.GetComponent<Button>(); }
        if (buyOneButton != null) buyOneButton.onClick.AddListener(() => WeaponSystem.Instance?.BuyOne());
        if (buyMaxButton != null) buyMaxButton.onClick.AddListener(() => WeaponSystem.Instance?.BuyMax());
    }

    void Update()
    {
        if (WeaponSystem.Instance == null || coreInfoText == null) return;
        double cost = WeaponSystem.Instance.GetCost();
        coreInfoText.text = $"Rusty Core Lv{WeaponSystem.Instance.coreLevel}\n+1 CPS per lvl (no DMG)\nNext: {HUD.Format(cost)} scrap";
    }
}
