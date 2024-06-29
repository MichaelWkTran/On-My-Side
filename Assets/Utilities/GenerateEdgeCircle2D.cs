using UnityEngine;

public class GenerateEdgeCircle2D : MonoBehaviour
{
    public EdgeCollider2D m_circleEdgeCollider2D;
    public LineRenderer m_lineRenderer;
    public float m_radius;
    public Vector2 m_offset;
    public uint m_pointNum;

    void OnValidate()
    {
        if (m_circleEdgeCollider2D == null) return;
        if (m_radius <= 0.0f) return;
        if (m_pointNum <= 0U) return;

        //Create a new array of Vector2 to hold the points
        Vector2[] points = new Vector2[m_pointNum];
        if (m_lineRenderer != null) m_lineRenderer.positionCount = points.Length;

        //Calculate the angle between each point
        float angleStep = 360.0f / m_pointNum;

        //Generate the points on the circle
        for (int i = 0; i < m_pointNum; i++)
        {
            //
            float angle = angleStep * i * Mathf.Deg2Rad;
            points[i] = new Vector2(Mathf.Cos(angle) * m_radius, Mathf.Sin(angle) * m_radius) +
                (Vector2)transform.position +
                m_offset;

            //
            if (m_lineRenderer != null)
            {
                m_lineRenderer.SetPosition(i, points[i]);
            }
        }

        //Assign the points to the PolygonCollider2D
        m_circleEdgeCollider2D.points = points;
    }
}
