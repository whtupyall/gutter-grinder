using UnityEngine;

/// <summary>
/// Code-built robot placeholder: replaces bunny art.
/// Body teal, black visor, cyan eyes with blink. No external sprites.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class RobotPlaceholder : MonoBehaviour
{
    public Color bodyColor = new Color(0.25f, 0.65f, 0.65f);
    public Color visorColor = Color.black;
    public Color eyeColor = new Color(0.3f, 1f, 1f);

    Transform leftEye, rightEye;
    float blinkTimer;
    PlayerController ctl;

    void Start()
    {
        ctl = GetComponent<PlayerController>();
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = MakeSquare();
        sr.color = bodyColor;
        transform.localScale = Vector3.one * 0.9f;

        // visor
        var visor = new GameObject("Visor");
        visor.transform.SetParent(transform);
        visor.transform.localPosition = new Vector3(0.15f, 0.12f, 0f);
        visor.transform.localScale = new Vector3(0.55f, 0.28f, 1f);
        var vsr = visor.AddComponent<SpriteRenderer>();
        vsr.sprite = MakeSquare();
        vsr.color = visorColor;
        vsr.sortingOrder = 1;

        // eyes
        leftEye = MakeEye("EyeL", new Vector3(-0.08f, 0f, 0f), visor.transform);
        rightEye = MakeEye("EyeR", new Vector3(0.12f, 0f, 0f), visor.transform);
    }

    Transform MakeEye(string n, Vector3 lp, Transform parent)
    {
        var e = new GameObject(n);
        e.transform.SetParent(parent);
        e.transform.localPosition = lp;
        e.transform.localScale = new Vector3(0.16f, 0.32f, 1f);
        var s = e.AddComponent<SpriteRenderer>();
        s.sprite = MakeSquare();
        s.color = eyeColor;
        s.sortingOrder = 2;
        return e.transform;
    }

    void Update()
    {
        blinkTimer += Time.deltaTime;
        if (blinkTimer > 2.5f)
        {
            // blink: squash eyes briefly
            leftEye.localScale = new Vector3(0.16f, 0.06f, 1f);
            rightEye.localScale = new Vector3(0.16f, 0.06f, 1f);
            if (blinkTimer > 2.65f)
            {
                leftEye.localScale = new Vector3(0.16f, 0.32f, 1f);
                rightEye.localScale = new Vector3(0.16f, 0.32f, 1f);
                blinkTimer = 0f;
            }
        }
        // run wobble only when actually moving; idle stance when blocked fighting
        bool fighting = ctl != null && ctl.blockCount > 0;
        if (fighting)
            transform.rotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 10f) * 2f);
    }

    Sprite MakeSquare()
    {
        Texture2D tex = new Texture2D(32, 32);
        for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
}
