using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSetupDiagnostics : MonoBehaviour
{
    public GameObject hedgeTilemapObject;   // Child holding Hedge tiles
    public GameObject pathwayTilemapObject; // Child holding Pathway tiles
    public AStarGridManager gridManager;

    private void Start()
    {
        Debug.Log("=== Grid Setup Diagnostic Report ===");

        // 1. Check hedge object setup
        if (hedgeTilemapObject == null)
        {
            Debug.LogError("❌ Hedge Tilemap object not assigned.");
        }
        else
        {
            var tilemap = hedgeTilemapObject.GetComponent<Tilemap>();
            var tilemapRenderer = hedgeTilemapObject.GetComponent<TilemapRenderer>();
            var tilemapCollider = hedgeTilemapObject.GetComponent<TilemapCollider2D>();
            var compositeCollider = hedgeTilemapObject.GetComponent<CompositeCollider2D>();
            var rb = hedgeTilemapObject.GetComponent<Rigidbody2D>();

            Debug.Log($"[Hedge] Layer = {LayerMask.LayerToName(hedgeTilemapObject.layer)}");
            Debug.Log($"[Hedge] Tilemap present: {(tilemap != null)}");
            Debug.Log($"[Hedge] TilemapRenderer present: {(tilemapRenderer != null)}");
            Debug.Log($"[Hedge] TilemapCollider2D present: {(tilemapCollider != null)}");
            Debug.Log($"[Hedge] CompositeCollider2D present: {(compositeCollider != null)}");
            Debug.Log($"[Hedge] Rigidbody2D present: {(rb != null)}, type: {(rb != null ? rb.bodyType.ToString() : "None")}");

            if (hedgeTilemapObject.layer != LayerMask.NameToLayer("Obstacles"))
                Debug.LogWarning("⚠️ Hedge object is not on 'Obstacles' layer!");
        }

        // 2. Check pathway object setup
        if (pathwayTilemapObject == null)
        {
            Debug.LogError("❌ Pathway Tilemap object not assigned.");
        }
        else
        {
            var tilemap = pathwayTilemapObject.GetComponent<Tilemap>();
            var tilemapRenderer = pathwayTilemapObject.GetComponent<TilemapRenderer>();
            var tilemapCollider = pathwayTilemapObject.GetComponent<TilemapCollider2D>();

            Debug.Log($"[Pathway] Layer = {LayerMask.LayerToName(pathwayTilemapObject.layer)}");
            Debug.Log($"[Pathway] Tilemap present: {(tilemap != null)}");
            Debug.Log($"[Pathway] TilemapRenderer present: {(tilemapRenderer != null)}");
            Debug.Log($"[Pathway] TilemapCollider2D present: {(tilemapCollider != null)} ❌ Should be false");

            if (pathwayTilemapObject.layer != LayerMask.NameToLayer("Default"))
                Debug.LogWarning("⚠️ Pathway object is not on 'Default' layer!");
        }

        // 3. Check AStarGridManager unwalkableMask
        if (gridManager == null)
        {
            Debug.LogError("❌ AStarGridManager not assigned.");
        }
        else
        {
            Debug.Log($"[GridManager] Node Radius = {gridManager.nodeRadius}");
            Debug.Log($"[GridManager] Grid World Size = {gridManager.gridWorldSize}");

            LayerMask mask = gridManager.unwalkableMask;
            string layerNames = "";
            for (int i = 0; i < 32; i++)
            {
                if (((1 << i) & mask) != 0)
                {
                    layerNames += LayerMask.LayerToName(i) + " ";
                }
            }
            Debug.Log($"[GridManager] UnwalkableMask includes: {layerNames}");

            if (!layerNames.Contains("Obstacles"))
                Debug.LogWarning("⚠️ UnwalkableMask does not include 'Obstacles'!");
        }

        Debug.Log("=== End of Report ===");
    }
}
