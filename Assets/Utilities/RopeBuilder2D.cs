#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

public class RopeBuilder2D : MonoBehaviour
{
    [SerializeField] HingeJoint2D m_segementPrefab;
    [SerializeField] float m_segmentOffset;
    [SerializeField] int m_segementCount;
    [SerializeField] bool m_axis;

    public void SpawnSegment()
    {
        HingeJoint2D lastSegment = null; try { lastSegment = GetComponentsInChildren<HingeJoint2D>()[^1]; } catch (Exception) { }
        HingeJoint2D segment = Instantiate(m_segementPrefab, transform);

        if (lastSegment == null) return;

        segment.transform.position = lastSegment.transform.position;
        segment.transform.position += (m_axis ? Vector3.right : -Vector3.up) * m_segmentOffset;
        segment.connectedBody = lastSegment.attachedRigidbody;
    }

    public void SpawnSegments()
    {
        for (int i = 0; i < m_segementCount; i++) SpawnSegment();
    }
}

[CustomEditor(typeof(RopeBuilder2D))]
public class RopeBuilder2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RopeBuilder2D ropeBuilder = (RopeBuilder2D)target;
        if (GUILayout.Button("Spawn Segments")) ropeBuilder.SpawnSegments();
    }
}

#endif

