using UnityEngine;

/// <summary>
/// Enemy that never hurts you. You touch it -> it locks and you auto-hit it.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Monster : MonoBehaviour
{
    public string monsterId = "grub";
    public int maxHP = 1;
    public double coinValue = 5;

    int hp;
    bool fighting;
    bool blocking;
    float hitTimer;
    float fightTime;
    Transform player;
    PlayerController playerCtl;

    void Start()
    {
        hp = maxHP;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) { player = p.transform; playerCtl = p.GetComponent<PlayerController>(); }
        MakeHealthBar();
    }

    void Update()
    {
        float speed = GameManager.Instance != null ? GameManager.Instance.runSpeed : 5f;

        if (!fighting)
        {
            // stationary in world: player runs TO bot, bot doesn't rush player
            // cleanup if left far behind (jumped over)
            if (player != null && player.position.x - transform.position.x > 15f)
                Destroy(gameObject);
        }
        else
        {
            // stick next to player while fighting
            if (player != null)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(player.position.x + 1.2f, transform.position.y, 0), 10f * Time.deltaTime);

            hitTimer += Time.deltaTime;
            fightTime += Time.deltaTime;
            // 15-sec rule [CONFIRMED]: regulars never stall longer. Pity kill at 12s to stay safely under.
            if (fightTime >= 12f) { TakeDamage(9999); return; }
            float interval = WeaponSystem.Instance != null ? WeaponSystem.Instance.attackInterval : 0.4f;
            if (hitTimer >= interval)
            {
                hitTimer = 0f;
                int dmg = WeaponSystem.Instance != null ? WeaponSystem.Instance.GetDamage() : 1;
                TakeDamage(dmg);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // re-find in case tag/component was fixed after Start
        if (playerCtl == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) { player = p.transform; playerCtl = p.GetComponent<PlayerController>(); }
        }
        // first hit instantly on touch: 1-HP = pass-through kill, no stop
        fighting = true;
        hitTimer = 0f;
        TakeDamage(WeaponSystem.Instance != null ? WeaponSystem.Instance.GetDamage() : 1);
        // survived = stop and fight
        if (hp > 0 && playerCtl != null && !blocking)
        {
            blocking = true;
            playerCtl.Block();
        }
        else if (hp <= 0)
        {
            fighting = false;
        }
    }

    void OnDestroy()
    {
        if (blocking && playerCtl != null) playerCtl.Unblock();
    }

    void TakeDamage(int dmg)
    {
        hp -= dmg;
        // tiny hop feedback
        transform.localScale = Vector3.one * 1.15f;
        Invoke(nameof(ResetScale), 0.08f);
        UpdateHealthBar();
        ShowDamage(dmg);
        if (hp <= 0) Die();
    }

    // --- test visuals: health bar + damage numbers, no art needed ---
    Transform hpFg;
    void MakeSquareSprite(Color c, Vector3 lp, Vector3 sc, int order, out Transform t)
    {
        var go = new GameObject("hp");
        go.transform.SetParent(transform);
        go.transform.localPosition = lp;
        go.transform.localScale = sc;
        var s = go.AddComponent<SpriteRenderer>();
        Texture2D tex = new Texture2D(4, 4);
        for (int x = 0; x < 4; x++) for (int y = 0; y < 4; y++) tex.SetPixel(x, y, Color.white);
        tex.Apply();
        s.sprite = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 32);
        s.color = c;
        s.sortingOrder = order;
        t = go.transform;
    }
    void MakeHealthBar()
    {
        Transform bg;
        MakeSquareSprite(new Color(0f, 0f, 0f, 0.7f), new Vector3(0f, 0.7f, 0f), new Vector3(1.1f, 0.14f, 1f), 10, out bg);
        MakeSquareSprite(Color.green, new Vector3(0f, 0.7f, 0f), new Vector3(1.0f, 0.09f, 1f), 11, out hpFg);
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        if (hpFg == null) return;
        float f = Mathf.Clamp01((float)hp / Mathf.Max(1, maxHP));
        hpFg.localScale = new Vector3(1.0f * f, 0.09f, 1f);
        hpFg.localPosition = new Vector3(-0.5f * (1f - f), 0.7f, 0f);
        var sr = hpFg.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = f > 0.5f ? Color.green : (f > 0.25f ? Color.yellow : Color.red);
    }
    void ShowDamage(int dmg)
    {
        var go = new GameObject("dmg");
        go.transform.position = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.9f, 0f);
        var tm = go.AddComponent<TextMesh>();
        tm.text = dmg.ToString();
        tm.fontSize = 48;
        tm.characterSize = 0.06f;
        tm.color = Color.white;
        tm.anchor = TextAnchor.MiddleCenter;
        go.AddComponent<DamagePopup>();
    }

    void ResetScale() { if (this != null) transform.localScale = Vector3.one; }

    void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(coinValue);
            GameManager.Instance.AddKill(monsterId);
        }
        Destroy(gameObject);
    }
}
