using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DynamicCone : MonoBehaviour
{
    [Header("Cone Settings")]
    public float angle = 45f;    // Öffnungswinkel in Grad
    public float radius = 2f;    // Radius des Kegels
    public int segments = 30;    // Anzahl der Unterteilungen

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateCone();
    }

    void Update()
    {
        CreateCone(); // Optional: Live-Update bei Änderung der Parameter
    }

    void CreateCone()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // Mittelpunkt (Spitze des Kegels)

        for (int i = 0; i <= segments; i++)
        {
            float fraction = (float)i / segments;
            float theta = Mathf.Deg2Rad * (fraction * angle - angle / 2f);

            // HIER Cos/Sin für richtige Ausrichtung
            vertices[i + 1] = new Vector3(Mathf.Cos(theta) * radius, 0f, Mathf.Sin(theta) * radius);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;         // Zentrum
            triangles[i * 3 + 1] = i + 1; // aktueller Randpunkt
            triangles[i * 3 + 2] = i + 2; // nächster Randpunkt
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void OnDrawGizmos()
    {
        if (mesh == null) return;

        Gizmos.color = Color.red;
        foreach (Vector3 v in mesh.vertices)
        {
            Gizmos.DrawSphere(transform.TransformPoint(v), 0.05f);
        }
    }
}
