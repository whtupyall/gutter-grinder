using UnityEngine;

/// <summary>Step 3b: distance-based evil bots, random gaps. No timer loop.</summary>
public class MonsterSpawner : MonoBehaviour
{
    public float minGap = 9f;
    public float maxGap = 18f;
    public float groundY = -2f;

    [Header("Art - drag evil robot sprite here (optional)")]
    public Sprite botSprite;

    float nextSpawnX = 14f;

    void Update()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p == null) return;
        var ctl = p.GetComponent<PlayerController>();
        if (ctl != null && ctl.blockCount > 0) return;

        float px = p.transform.position.x;
        if (nextSpawnX < px) nextSpawnX = px + 14f;
        if (px + 14f >= nextSpawnX)
        {
            SpawnBot(nextSpawnX);
            nextSpawnX += Random.Range(minGap, maxGap);
        }
    }

    void SpawnBot(float x)
    {
        GameObject go = new GameObject("scrapbot");
        go.transform.position = new Vector3(x, groundY, 0);

        var sr = go.AddComponent<SpriteRenderer>();
        if (botSprite != null) { sr.sprite = botSprite; sr.color = Color.white; }
        else
        {
            sr.color = new Color(1f, 0.3f, 0.25f);
            sr.sprite = MakeSquareSprite();
        }
        go.transform.localScale = Vector3.one * 0.7f;

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = Vector2.one;

        var m = go.AddComponent<Monster>();
        m.monsterId = "scrapbot";
        m.maxHP = 1;
        m.coinValue = 5;
    }

    Sprite MakeSquareSprite()
    {
        Texture2D tex = new Texture2D(32, 32);
        for (int px = 0; px < 32; px++) for (int py = 0; py < 32; py++) tex.SetPixel(px, py, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
}
