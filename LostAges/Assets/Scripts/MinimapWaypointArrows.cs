using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class MinimapWaypointArrows : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera; // Kamera der Minimap
    [SerializeField] private RectTransform minimapBounds; // Grenzen der Minimap (UI-Element)
    [SerializeField] private GameObject arrowPrefab; // Prefab für Pfeile
    [SerializeField] private Transform arrowParent; // Eltern-Transform für Ordnung
    [SerializeField] private float arrowOffset = 5f; // Abstand der Pfeile vom Rand der Minimap

    private List<GameObject> arrows = new List<GameObject>(); // Liste der aktiven Pfeile
    [SerializeField] private TopDownCharacterController playerController;
    private float minimapZoomFactor = 1f;

    private void Update()
    {
        minimapZoomFactor = minimapCamera.orthographicSize;
        UpdateArrows();
    }

    private void UpdateArrows()
    {
        
    }

   
}
