using UnityEngine;

/// <summary>
/// Step 3: spawns only ground evil bots (1 HP). Zones/flyers come later.
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    public float spawnInterval = 2.2f;
    public float spawnX = 12f;
    public float groundY = -2f;

    [Header("Art - drag evil robot sprite here (optional)")]
    public Sprite botSprite;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBot();
        }
    }

    void SpawnBot()
    {
        float x = spawnX;
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) x = p.transform.position.x + 12f;
        GameObject go = new GameObject("scrapbot");
        go.transform.position = new Vector3(x, groundY, 0);

        var sr = go.AddComponent<SpriteRenderer>();
        if (botSprite != null) { sr.sprite = botSprite; sr.color = Color.white; }
        else
        {
            sr.color = new Color(1f, 0.3f, 0.25f); // red evil bot placeholder
            sr.sprite = MakeSquareSprite();
        }
        go.transform.localScale = Vector3.one * 0.7f;

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = Vector2.one;

        var m = go.AddComponent<Monster>();
        m.monsterId = "scrapbot";
        m.maxHP = 3;
        m.coinValue = 5;
    }

    Sprite MakeSquareSprite()
    {
        Texture2D tex = new Texture2D(32, 32);
        for (int x = 0; x < 32; x++) for (int y = 0; y < 32; y++) tex.SetPixel(x, y, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
}
