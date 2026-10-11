using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float y = 0f;

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = new Vector3(target.position.x + 4f, y, -10f);
    }
}
