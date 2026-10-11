using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 16f;
    public float coyoteTime = 0.12f;
    public float jumpBuffer = 0.12f;

    Rigidbody2D rb;
    bool grounded;
    int groundTouches;
    float lastGrounded = -99f, lastJumpPressed = -99f;
    bool jumpHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 2.2f;
    }

    void Start()
    {
        GameObject jb = GameObject.Find("JumpButton");
        if (jb != null)
        {
            var btn = jb.GetComponent<UnityEngine.UI.Button>();
            if (btn != null) btn.onClick.AddListener(TryJump);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            lastJumpPressed = Time.time;
            jumpHeld = true;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            lastJumpPressed = Time.time;
            jumpHeld = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            jumpHeld = false;
            if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            jumpHeld = false;
            if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        // all touches gone = not held (button taps use TryJump, handled below)
        if (Input.touchCount == 0 && Input.GetMouseButton(0) == false && Input.GetKey(KeyCode.Space) == false)
            jumpHeld = false;

        if (grounded) lastGrounded = Time.time;

        if (Time.time - lastJumpPressed < jumpBuffer && Time.time - lastGrounded < coyoteTime)
        {
            // buffered tap released before landing = short hop, not full height
            float force = jumpHeld ? jumpForce : jumpForce * 0.55f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
            lastJumpPressed = -99f;
            lastGrounded = -99f;
            grounded = false;
            groundTouches = 0;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    }

    public void TryJump()
    {
        lastJumpPressed = Time.time;
        jumpHeld = true; // UI Button has no release signal, so treat as full hold for now
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground")) { groundTouches++; grounded = true; }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground"))
        {
            groundTouches = Mathf.Max(0, groundTouches - 1);
            if (groundTouches == 0) grounded = false;
        }
    }
}
