using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ForceField : MonoBehaviour
{
    public Vector2 m_linearForce; //The amount of force applied to colliding rigidbodies at a linear direction
    public AnimationCurve m_radialForce; //The amount of force applied to colliding rigidbodies from its origin
    public LayerMask m_includeLayerMask; //The rigidbodies that the field can interact with
    HashSet<Rigidbody2D> m_enteredRigidbodies = new HashSet<Rigidbody2D>(); //The rigidbodies currently colliding with

    void FixedUpdate()
    {
        //Apply linear force
        if (m_linearForce != Vector2.zero) foreach (Rigidbody2D rigidbody in m_enteredRigidbodies) rigidbody.AddForce(m_linearForce);

        //Apply radial force
        foreach (Rigidbody2D rigidbody in m_enteredRigidbodies)
        {
            Vector2 difference = rigidbody.position - (Vector2)transform.position;
            rigidbody.AddForce(difference.normalized * m_radialForce.Evaluate(difference.magnitude));
        }
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (m_includeLayerMask != (m_includeLayerMask | (1 << _collision.gameObject.layer))) return;

        Rigidbody2D rb = _collision.attachedRigidbody;
        if (rb != null) m_enteredRigidbodies.Add(rb);
    }

    void OnTriggerExit2D(Collider2D _collision)
    {
        if (m_includeLayerMask != (m_includeLayerMask | (1 << _collision.gameObject.layer))) return;
        
        Rigidbody2D rb = _collision.attachedRigidbody;
        if (rb != null) m_enteredRigidbodies.Remove(rb);
    }
}
