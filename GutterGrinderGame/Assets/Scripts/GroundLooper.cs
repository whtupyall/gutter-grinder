using UnityEngine;

/// <summary>
/// Endless ground: recycles Ground-tagged tiles ahead of player.
/// Setup: 3 Ground squares side-by-side, same Y, Tag=Ground, BoxCollider2D.
/// </summary>
public class GroundLooper : MonoBehaviour
{
    public Transform player;
    public float tileWidth = 30f;
    public float despawnMargin = 10f;

    GameObject[] tiles;
    int numTiles;

    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Ground");
        numTiles = Mathf.Max(1, tiles.Length);
        // auto-detect width from first tile if it has a collider
        if (tiles.Length > 0)
        {
            var col = tiles[0].GetComponent<BoxCollider2D>();
            if (col != null)
                tileWidth = col.bounds.size.x;
        }
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || tiles == null) return;
        foreach (var t in tiles)
        {
            if (t == null) continue;
            // right edge of tile
            float rightEdge = t.transform.position.x + tileWidth / 2f;
            if (player.position.x - despawnMargin > rightEdge)
            {
                t.transform.position += new Vector3(tileWidth * numTiles, 0f, 0f);
            }
        }
    }
}
