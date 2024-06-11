using System;
using UnityEngine;

[RequireComponent(typeof(Observer))]
public class ShatterObject2D : MonoBehaviour
{
    public float m_breakImpulse;
    public GameObject m_breakPrefab;
    [SerializeField] Observer m_observer;

    void OnCollisionEnter2D(Collision2D _collision)
    {
        //Calculate the total impulse
        Vector2 totalImpulse = Vector2.zero;
        foreach (ContactPoint2D contactPoint in _collision.contacts) totalImpulse += contactPoint.normal * contactPoint.normalImpulse;

        //Do not break the object if the total impulse is below breaking threshold
        if (totalImpulse.sqrMagnitude < m_breakImpulse * m_breakImpulse) return;

        //Destroy the object
        m_observer.m_onNotify?.Invoke();
        if (m_breakPrefab != null) Instantiate(m_breakPrefab, transform.position, transform.rotation).SetActive(true);
        Destroy(gameObject);
    }
}
