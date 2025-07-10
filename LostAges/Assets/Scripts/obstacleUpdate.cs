using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Collider2D))]
public class obstacleUpdate : MonoBehaviour
{
    private Bounds lastBounds;

    void Start()
    {
        UpdateGraph(); // Initial
        lastBounds = GetComponent<BoxCollider2D>().bounds;
    }

    void Update()
    {
        Bounds currentBounds = GetComponent<BoxCollider2D>().bounds;

        // Nur aktualisieren, wenn sich etwas geändert hat
        if (currentBounds != lastBounds)
        {
            UpdateGraph();
            lastBounds = currentBounds;
        }
    }

    void OnEnable()
    {
        UpdateGraph(); // Wenn Objekt aktiviert wird
    }

    void OnDisable()
    {
        UpdateGraph(); // Wenn Objekt deaktiviert wird
    }

    private void UpdateGraph()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;

        Bounds bounds = col.bounds;
        GraphUpdateObject guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);

        Debug.Log($"Graph updated for obstacle '{gameObject.name}'"); //DEBUG
    }
}
