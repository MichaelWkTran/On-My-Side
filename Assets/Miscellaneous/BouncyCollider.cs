using UnityEngine;

public class BouncyCollider : MonoBehaviour
{
    public float m_bounceScale = 1.0f;
    public float m_minImpulse = 0.0f;
    public float m_maxImpulse = 1.0f;

    void OnCollisionEnter2D(Collision2D _collision)
    {
        Rigidbody2D rb = _collision.rigidbody;
        if (rb == null) return;

        float impulseMagnitude = _collision.contacts[0].normalImpulse * m_bounceScale;
        impulseMagnitude = Mathf.Clamp(impulseMagnitude, m_minImpulse, m_maxImpulse);
        Vector2 bounceImpulse = -_collision.contacts[0].normal * impulseMagnitude;

        rb.AddForce(bounceImpulse, ForceMode2D.Impulse);
    }
}
