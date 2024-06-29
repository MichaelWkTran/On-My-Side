using UnityEngine;

public class ShatterObject : MonoBehaviour
{
    public float m_breakImpulse;
    public GameObject m_breakPrefab;

    void OnCollisionEnter(Collision _collision)
    {
        //Do not break the object if the total impulse is below breaking threshold
        if (_collision.impulse.sqrMagnitude < m_breakImpulse * m_breakImpulse) return;

        //Destroy the object
        if (m_breakPrefab != null) Instantiate(m_breakPrefab, transform.position, transform.rotation).SetActive(true);
        Destroy(gameObject);
    }
}
