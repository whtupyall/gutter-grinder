using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public float spawnInterval = 1.4f;
    public float spawnX = 12f;
    [Header("Art - drag coin sprite here (optional)")]
    public Sprite coinSprite;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval) { timer = 0f; SpawnArc(); }
    }

    void SpawnArc()
    {
        // 5-coin arc at random height: ground / low / high
        float[] ys = { -2f, 0f, 1.8f };
        float y = ys[Random.Range(0, ys.Length)];
        for (int i = 0; i < 5; i++)
        {
            GameObject go = new GameObject("Coin");
            go.transform.position = new Vector3(spawnX + i * 0.9f, y + Mathf.Sin(i * 0.8f) * 0.5f, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            if (coinSprite != null) { sr.sprite = coinSprite; sr.color = Color.white; }
            else { sr.color = Color.yellow; sr.sprite = MakeCircleSprite(); }
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
