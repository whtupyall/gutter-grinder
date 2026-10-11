using UnityEngine;

/// <summary>Floating damage number: rises and fades, then destroys. Test visual only.</summary>
public class DamagePopup : MonoBehaviour
{
    float life;
    TextMesh tm;
    void Start() { tm = GetComponent<TextMesh>(); }
    void Update()
    {
        life += Time.deltaTime;
        transform.position += Vector3.up * Time.deltaTime * 1.5f;
        if (tm != null) tm.color = new Color(1f, 1f, 1f, 1f - life);
        if (life > 0.7f) Destroy(gameObject);
    }
}
