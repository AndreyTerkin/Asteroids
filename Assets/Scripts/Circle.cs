using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour
{
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius = 1.0f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        InitCircle();
    }

    private void InitCircle()
    {
        lineRenderer.widthMultiplier = lineWidth;
        Vector3[] vertices = GetVertices();
        lineRenderer.positionCount = vertexCount;
        lineRenderer.SetPositions(vertices);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3[] vertices = GetVertices();
        for(int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(transform.position + vertices[i-1], transform.position + vertices[i]);
        }
    }
#endif

    private Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[vertexCount];

        float deltaTheta = (2 * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount; i++)
        {
            vertices[i] = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            theta += deltaTheta;
        }
        return vertices;
    }
}
