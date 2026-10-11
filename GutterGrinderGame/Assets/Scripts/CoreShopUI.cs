using UnityEngine;
using UnityEngine.UI;

/// <summary>Test Core shop: builds 3 rows (name/lv/cost + Buy1/BuyMax). Values placeholder.</summary>
public class CoreShopUI : MonoBehaviour
{
    struct Row { public Text label; public Button buy1; public Button buyMax; }
    Row[] rows;
    Canvas canvas;

    void Start()
    {
        var cgo = new GameObject("CoreCanvas");
        cgo.transform.SetParent(transform);
        canvas = cgo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5;
        cgo.AddComponent<CanvasScaler>();
        cgo.AddComponent<GraphicRaycaster>();

        var panel = new GameObject("CorePanel");
        panel.transform.SetParent(cgo.transform);
        var prt = panel.AddComponent<RectTransform>();
        prt.anchorMin = new Vector2(1f, 0.5f); prt.anchorMax = new Vector2(1f, 0.5f);
        prt.pivot = new Vector2(1f, 0.5f); prt.anchoredPosition = new Vector2(-12f, 0f);
        prt.sizeDelta = new Vector2(420f, 300f);
        var pimg = panel.AddComponent<Image>();
        pimg.color = new Color(0.1f, 0.1f, 0.12f, 0.92f);

        rows = new Row[3];
        for (int i = 0; i < 3; i++)
        {
            int idx = i;
            var rgo = new GameObject($"Row{i}");
            rgo.transform.SetParent(panel.transform);
            var rrt = rgo.AddComponent<RectTransform>();
            rrt.anchorMin = new Vector2(0f, 1f); rrt.anchorMax = new Vector2(1f, 1f);
            rrt.pivot = new Vector2(0.5f, 1f); rrt.anchoredPosition = new Vector2(0f, -10f - i * 95f);
            rrt.sizeDelta = new Vector2(400f, 85f);

            var lgo = new GameObject("Label");
            lgo.transform.SetParent(rgo.transform);
            var lrt = lgo.AddComponent<RectTransform>();
            lrt.anchorMin = new Vector2(0f, 0f); lrt.anchorMax = new Vector2(0.62f, 1f);
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            var lt = lgo.AddComponent<Text>();
            lt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            lt.fontSize = 20; lt.color = Color.white;
            rows[i].label = lt;

            var b1 = MakeBtn(rgo.transform, "Buy1", "BUY 1", new Vector2(0.63f, 0.05f));
            b1.onClick.AddListener(() => WeaponSystem.Instance?.BuyOne(idx));
            var bm = MakeBtn(rgo.transform, "BuyMax", "MAX", new Vector2(0.82f, 0.05f));
            bm.onClick.AddListener(() => WeaponSystem.Instance?.BuyMax(idx));
        }
    }

    Button MakeBtn(Transform parent, string n, string t, Vector2 anchor)
    {
        var go = new GameObject(n);
        go.transform.SetParent(parent);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchor; rt.anchorMax = new Vector2(anchor.x + 0.17f, 0.95f);
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        var img = go.AddComponent<Image>(); img.color = new Color(0.2f, 0.7f, 0.25f);
        var b = go.AddComponent<Button>();
        var tgo = new GameObject("T"); tgo.transform.SetParent(go.transform);
        var trt = tgo.AddComponent<RectTransform>();
        trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
        trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
        var tx = tgo.AddComponent<Text>();
        tx.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        tx.fontSize = 18; tx.alignment = TextAnchor.MiddleCenter; tx.color = Color.white; tx.text = t;
        return b;
    }

    void Update()
    {
        if (WeaponSystem.Instance == null || rows == null) return;
        var parts = WeaponSystem.Instance.parts;
        for (int i = 0; i < rows.Length && i < parts.Count; i++)
        {
            var p = parts[i];
            // reveal rule placeholder: show row if affordable-ish (owned >= 40% of price) else ???
            double scrap = GameManager.Instance != null ? GameManager.Instance.coins : 0;
            bool revealed = scrap >= p.baseCost * 0.4 || p.level > 0;
            rows[i].label.text = revealed
                ? $"{p.DisplayName()} Lv{p.level}\n+{p.GetCPS():F1} CPS  Next:{p.GetCost():F0}"
                : $"??? (earn scrap)";
        }
    }
}
