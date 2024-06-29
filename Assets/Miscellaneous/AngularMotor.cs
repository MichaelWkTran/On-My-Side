using UnityEngine;

public class AngularMotor : MonoBehaviour
{
    public Rigidbody2D m_rigidbody;
    public float m_startAngularVelocity = 0.0f;
    public float m_angularAcceleration = 10.0f;
    public float m_maxAngularVelocity = 20.0f;

    void Start()
    {
        m_rigidbody.angularVelocity = m_startAngularVelocity;
    }

    void FixedUpdate()
    {
        m_rigidbody.angularVelocity = Mathf.MoveTowards(m_rigidbody.angularVelocity, m_maxAngularVelocity, m_angularAcceleration);
    }
}
