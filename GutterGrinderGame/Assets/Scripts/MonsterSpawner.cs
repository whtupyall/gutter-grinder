using UnityEngine;

/// <summary>
/// Spawns placeholder monsters (no prefabs needed for Part 1).
/// Grub ground 1HP, Shroom unlocks after 10 grub kills.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    public float spawnInterval = 2.2f;
    public float spawnX = 12f;
    public float groundY = -2f;
    public float lowAirY = 0f;

    [Header("Art - drag sprites here to replace squares (optional)")]
    public Sprite grubSprite;
    public Sprite shroomSprite;
    public Sprite baticSprite;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        bool shroomUnlocked = QuestSystem.Instance != null && QuestSystem.Instance.shroomUnlocked;
        int zone = ZoneSystem.Instance != null ? ZoneSystem.Instance.currentZone : 0;
        int bonusHP = ZoneSystem.Instance != null ? ZoneSystem.Instance.BonusHP() : 0;

        // Zone 1+ introduces Batic flyer (3 HP base). Higher zones = more of them.
        float baticChance = zone == 0 ? 0f : (zone == 1 ? 0.3f : 0.4f);
        float roll = Random.value;
        string pick = "grub";
        if (roll < baticChance) pick = "batic";
        else if (shroomUnlocked && roll < baticChance + 0.35f) pick = "shroom";

        GameObject go = new GameObject(pick);
        float y = groundY;
        if (pick == "shroom") y = lowAirY;
        if (pick == "batic") y = 1.8f; // high-air, needs full jump

        go.transform.position = new Vector3(spawnX, y, 0);

        var sr = go.AddComponent<SpriteRenderer>();
        Sprite custom = pick == "batic" ? baticSprite : (pick == "shroom" ? shroomSprite : grubSprite);
        if (custom != null) { sr.sprite = custom; sr.color = Color.white; }
        else
        {
            if (pick == "batic") sr.color = new Color(0.6f, 0.4f, 1f);
            else sr.color = pick == "shroom" ? new Color(1f, 0.5f, 0.1f) : new Color(0.3f, 0.9f, 0.3f);
            sr.sprite = MakeSquareSprite();
        }
        go.transform.localScale = Vector3.one * (pick == "batic" ? 0.8f : (pick == "shroom" ? 0.9f : 0.7f));

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = Vector2.one;

        var m = go.AddComponent<Monster>();
        if (pick == "batic") { m.monsterId = "batic"; m.maxHP = 3 + bonusHP; m.coinValue = 25; }
        else if (pick == "shroom") { m.monsterId = "shroom"; m.maxHP = 2 + bonusHP; m.coinValue = 12; }
        else { m.monsterId = "grub"; m.maxHP = 1 + bonusHP; m.coinValue = 5; }
    }

    Sprite MakeSquareSprite()
    {
        Texture2D tex = new Texture2D(32, 32);
        for (int x = 0; x < 32; x++) for (int y = 0; y < 32; y++) tex.SetPixel(x, y, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
}
