using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] ParticleSystem m_collectParticle;
    bool m_collected = false;

    void Start()
    {
        m_collectParticle.gameObject.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.transform.tag == "Player" && m_collected == false) Collect();
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.transform.tag == "Player" && m_collected == false) Collect();
    }

    void Collect()
    {
        if (m_collected) return;
        m_collected = true;

        m_collectParticle.transform.SetParent(null);
        m_collectParticle.gameObject.SetActive(true);
        GameMode.m_instance.AddStar();
        Destroy(gameObject);
    }
}
