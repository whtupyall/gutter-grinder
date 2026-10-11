using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public TMP_Text daggerInfoText;
    public Button buyOneButton;
    public Button buyMaxButton;

    void Start()
    {
        if (daggerInfoText == null) { GameObject go = GameObject.Find("DaggerText"); if (go != null) daggerInfoText = go.GetComponent<TMP_Text>(); }
        if (buyOneButton == null) { GameObject go = GameObject.Find("Buy1Button"); if (go != null) buyOneButton = go.GetComponent<Button>(); }
        if (buyMaxButton == null) { GameObject go = GameObject.Find("BuyMaxButton"); if (go != null) buyMaxButton = go.GetComponent<Button>(); }
        if (buyOneButton != null) buyOneButton.onClick.AddListener(() => WeaponSystem.Instance?.BuyOne());
        if (buyMaxButton != null) buyMaxButton.onClick.AddListener(() => WeaponSystem.Instance?.BuyMax());
    }

    void Update()
    {
        if (WeaponSystem.Instance == null || daggerInfoText == null) return;
        double cost = WeaponSystem.Instance.GetCost();
        daggerInfoText.text = $"Rusty Dagger Lv{WeaponSystem.Instance.daggerLevel}\n+1 DMG +1 CPS per lvl\nNext: {HUD.Format(cost)} coins";
    }
}
