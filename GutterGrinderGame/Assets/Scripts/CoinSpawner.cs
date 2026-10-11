using UnityEngine;

/// <summary>Step 4b: distance-based coins, random gaps. No timer loop.</summary>
public class CoinSpawner : MonoBehaviour
{
    public float minGap = 6f;
    public float maxGap = 14f;

    float nextSpawnX = 12f;

    void Update()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p == null) return;
        var ctl = p.GetComponent<PlayerController>();
        if (ctl != null && ctl.blockCount > 0) return;

        float px = p.transform.position.x;
        if (px + 12f >= nextSpawnX)
        {
            SpawnCoins(nextSpawnX);
            nextSpawnX += Random.Range(minGap, maxGap);
        }
        // init on first run
        if (nextSpawnX < px) nextSpawnX = px + 12f;
    }

    void SpawnCoins(float x)
    {
        float y = -2f;
        for (int i = 0; i < 2; i++)
        {
            GameObject go = new GameObject("Coin");
            go.transform.position = new Vector3(x + i * 0.9f, y + Mathf.Sin(i * 0.8f) * 0.4f, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = Color.yellow;
            sr.sprite = MakeCircleSprite();
            go.transform.localScale = Vector3.one * 0.45f;
            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true; col.radius = 0.6f;
            var c = go.AddComponent<Coin>();
            c.value = 2;
        }
    }

    Sprite MakeCircleSprite()
    {
        int s = 32;
        Texture2D tex = new Texture2D(s, s);
        for (int x = 0; x < s; x++) for (int y = 0; y < s; y++)
        {
            float dx = x - s / 2f, dy = y - s / 2f;
            tex.SetPixel(x, y, (dx * dx + dy * dy < 240) ? Color.white : Color.clear);
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, s, s), new Vector2(0.5f, 0.5f), s);
    }
}
