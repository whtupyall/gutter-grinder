using UnityEngine;

/// <summary>
/// Fixed X runner. Tap/space to jump, hold for higher.
/// Simple: grounded via touching Ground tag. No GroundCheck object needed.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 11f;

    Rigidbody2D rb;
    bool grounded;
    int groundTouches;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Start()
    {
        // Auto-wire JumpButton so no OnClick setup needed
        GameObject jb = GameObject.Find("JumpButton");
        if (jb != null)
        {
            var btn = jb.GetComponent<UnityEngine.UI.Button>();
            if (btn != null) btn.onClick.AddListener(TryJump);
        }
    }

    void Update()
    {
        // Jump: Space, mouse, touch (old input - needs Active Input Handling = Both)
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) jumpPressed = true;
        if (jumpPressed) TryJump();

        // Variable jump: release early = shorter hop
        bool jumpReleased = Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0);
        if (jumpReleased && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    public void TryJump()
    {
        if (!grounded) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        grounded = false;
        groundTouches = 0;
        // squash stretch
        transform.localScale = new Vector3(0.7f, 1.1f, 1) * 0.7f;
        CancelInvoke(nameof(ResetSquash));
        Invoke(nameof(ResetSquash), 0.15f);
    }

    void ResetSquash() { if (this != null) transform.localScale = Vector3.one * 0.7f; }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground")) { groundTouches++; grounded = true; }
    }
    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground")) { groundTouches = Mathf.Max(0, groundTouches - 1); if (groundTouches == 0) grounded = false; }
    }
}
