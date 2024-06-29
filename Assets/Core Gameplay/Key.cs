using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public delegate void OnKeyCollectDelegate(Key _key); public OnKeyCollectDelegate m_onKeyCollect;
    //[SerializeField] ParticleSystem m_collectParticle;

    void Start()
    {
        //m_collectParticle.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.transform.tag != "Player") return;

        //m_collectParticle.transform.SetParent(null);
        //m_collectParticle.gameObject.SetActive(true);
        m_onKeyCollect?.Invoke(this);
        Destroy(gameObject);
    }
}
