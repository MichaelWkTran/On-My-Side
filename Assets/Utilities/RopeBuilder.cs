#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

public class RopeBuilder : MonoBehaviour
{
    [SerializeField] HingeJoint m_segementPrefab;
    [SerializeField] float m_segmentOffset;
    [SerializeField] int m_segementCount;
    [SerializeField] bool m_axis;

    public void SpawnSegment()
    {
        HingeJoint lastSegment = null; try { lastSegment = GetComponentsInChildren<HingeJoint>()[^1]; } catch (Exception) { }
        HingeJoint segment = Instantiate(m_segementPrefab, transform);

        if (lastSegment == null) return;

        segment.transform.position = lastSegment.transform.position;
        segment.transform.position += (m_axis ? Vector3.right : -Vector3.up) * m_segmentOffset;
        segment.connectedBody = lastSegment.GetComponent<Rigidbody>();
    }

    public void SpawnSegments()
    {
        for (int i = 0; i < m_segementCount; i++) SpawnSegment();
    }
}

[CustomEditor(typeof(RopeBuilder))]
public class RopeBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RopeBuilder ropeBuilder = (RopeBuilder)target;
        if (GUILayout.Button("Spawn Segments")) ropeBuilder.SpawnSegments();
    }
}

#endif

