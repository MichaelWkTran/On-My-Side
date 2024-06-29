using UnityEngine;
using PathCreation;

public class PathContraint : MonoBehaviour
{
    public Rigidbody2D m_rigidbody;
    public PathCreator m_pathCreator;
    public bool m_alignRotation;

    void FixedUpdate()
    {
        float closestTime = m_pathCreator.path.GetClosestTimeOnPath(m_rigidbody.position);
        m_rigidbody.position = m_pathCreator.path.GetPointAtTime(closestTime);
        if (m_alignRotation) m_rigidbody.rotation = Vector2.SignedAngle(Vector2.right, m_pathCreator.path.GetDirection(closestTime));
    }
}
