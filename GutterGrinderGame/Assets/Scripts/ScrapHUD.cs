using UnityEngine;
using UnityEngine.UI;

/// <summary>Step 4 test HUD: builds itself, no manual Canvas needed. Shows Scrap/Kills/DMG.</summary>
public class ScrapHUD : MonoBehaviour
{
    Text scrapText;
    void Start()
    {
        var canvasGo = new GameObject("ScrapCanvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        var txtGo = new GameObject("ScrapText");
        txtGo.transform.SetParent(canvasGo.transform);
        scrapText = txtGo.AddComponent<Text>();
        scrapText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        scrapText.fontSize = 28;
        scrapText.color = Color.white;
        var rt = txtGo.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(16f, -16f);
        rt.sizeDelta = new Vector2(600f, 120f);
    }

    void Update()
    {
        if (scrapText == null) return;
        double scrap = GameManager.Instance != null ? GameManager.Instance.coins : 0;
        int kills = GameManager.Instance != null ? GameManager.Instance.totalKills : 0;
        int dmg = WeaponSystem.Instance != null ? WeaponSystem.Instance.GetDamage() : 1;
        scrapText.text = $"Scrap: {scrap:F0}   Kills: {kills}   DMG: {dmg}\n(R = wipe save)";
    }
}
