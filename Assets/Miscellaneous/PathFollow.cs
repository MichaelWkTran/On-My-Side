using PathCreation;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public PathCreator m_pathCreator;
    public EndOfPathInstruction m_endOfPathInstruction;
    public float m_speed = 5;
    public bool m_isFixedUpdate = false;
    private float m_distanceTravelled;

    void Start() { if (m_pathCreator != null) { m_pathCreator.pathUpdated += OnPathChanged; OnPathChanged(); } }

    void UpdateOrFixedUpdate()
    {
        if (m_pathCreator == null) return;

        float deltaTime = m_isFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime;
        m_distanceTravelled += m_speed * deltaTime;
        m_distanceTravelled %= m_pathCreator.path.length * 2.0f;

        transform.position = m_pathCreator.path.GetPointAtDistance(m_distanceTravelled, m_endOfPathInstruction);
        transform.rotation = m_pathCreator.path.GetRotationAtDistance(m_distanceTravelled, m_endOfPathInstruction);
    }

    void Update() { if (!m_isFixedUpdate) UpdateOrFixedUpdate(); }

    void FixedUpdate() { if (m_isFixedUpdate) UpdateOrFixedUpdate(); }

    private void OnPathChanged() { m_distanceTravelled = m_pathCreator.path.GetClosestDistanceAlongPath(transform.position); }
}
