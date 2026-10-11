using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Coin : MonoBehaviour
{
    public double value = 1;
    float t;
    Transform player;
    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }
    void Update()
    {
        // stationary scrap: player runs TO it, auto-collect on pass (no rushing)
        if (player != null && player.position.x - transform.position.x > 15f)
        {
            Destroy(gameObject);
            return;
        }
        // juice: pulse + spin (works with squares until art arrives)
        t += Time.deltaTime * 6f;
        float s = 1f + Mathf.Sin(t) * 0.12f;
        transform.localScale = Vector3.one * 0.45f * s;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t * 0.5f) * 15f);
        if (transform.position.x < -15f) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null) GameManager.Instance.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
