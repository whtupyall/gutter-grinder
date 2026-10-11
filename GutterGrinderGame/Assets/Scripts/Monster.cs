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
    float hitTimer;
    Transform player;

    void Start()
    {
        hp = maxHP;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        float speed = GameManager.Instance != null ? GameManager.Instance.runSpeed : 5f;

        if (!fighting)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x < -15f) Destroy(gameObject);
        }
        else
        {
            // stick next to player while fighting
            if (player != null)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(player.position.x + 1.2f, transform.position.y, 0), 10f * Time.deltaTime);

            hitTimer += Time.deltaTime;
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
        if (other.CompareTag("Player")) fighting = true;
    }

    void TakeDamage(int dmg)
    {
        hp -= dmg;
        // tiny hop feedback
        transform.localScale = Vector3.one * 1.15f;
        Invoke(nameof(ResetScale), 0.08f);
        if (hp <= 0) Die();
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
