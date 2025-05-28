using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCenterPrinter : MonoBehaviour
{
    public Tilemap targetTilemap;

    void Start()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("❌ Please assign a Tilemap to 'targetTilemap'.");
            return;
        }

        BoundsInt bounds = targetTilemap.cellBounds;

        Vector3Int min = new Vector3Int(int.MaxValue, int.MaxValue, 0);
        Vector3Int max = new Vector3Int(int.MinValue, int.MinValue, 0);

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (targetTilemap.HasTile(pos))
            {
                min.x = Mathf.Min(min.x, pos.x);
                min.y = Mathf.Min(min.y, pos.y);
                max.x = Mathf.Max(max.x, pos.x);
                max.y = Mathf.Max(max.y, pos.y);
            }
        }

        Vector3 worldMin = targetTilemap.CellToWorld(min);
        Vector3 worldMax = targetTilemap.CellToWorld(max) + targetTilemap.layoutGrid.cellSize;
        Vector3 worldCenter = (worldMin + worldMax) / 2f;
        Vector2 worldSize = new Vector2(worldMax.x - worldMin.x, worldMax.y - worldMin.y);

        Debug.Log($"✅ Real World Min: {worldMin}");
        Debug.Log($"✅ Real World Max: {worldMax}");
        Debug.Log($"🎯 Real World Center: {worldCenter}");
        Debug.Log($"📏 Real World Size: {worldSize}");
    }
}
