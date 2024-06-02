#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class PolygonToMeshCollider : MonoBehaviour
{
    public PolygonCollider2D m_sourcePolyCollider; //The polygon collider with data used for generating the collider mesh
    public MeshCollider m_targetMeshCollider; //The mesh collider from which the generated mesh from the polygon collider is assigned to
    public MeshFilter m_targetMeshFilter; //The mesh filter from which the generated mesh from the polygon collider is assigned to
    public float m_frontDistance = -0.5f;
    public float m_backDistance = 0.5f;

    public void AssignMesh()
    {
        Mesh generatedMesh = ExtrudeSprite.CreateMesh(m_sourcePolyCollider.points, m_frontDistance, m_backDistance);
        if (m_targetMeshCollider != null) m_targetMeshCollider.sharedMesh = generatedMesh;
        if (m_targetMeshFilter != null)   m_targetMeshFilter.sharedMesh = generatedMesh;
    }
}

[CustomEditor(typeof(PolygonToMeshCollider))]
public class PolygonToMeshColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PolygonToMeshCollider polygonToMeshCollider = (PolygonToMeshCollider)target;

        if (GUILayout.Button("Assign Mesh")) polygonToMeshCollider.AssignMesh();
    }
}

#endif