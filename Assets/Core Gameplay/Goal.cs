using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public List<Key> m_keys;
    public UnityEvent m_unlockEvent;

    void Start()
    {
        
        if (m_keys != null && m_keys.Count > 0)
        {
            //Lock the goal if keys are required to unlock the goal
            foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = false;
            
            //Assign delegate to keys to signal whether the goal should be unlocked
            foreach (Key key in m_keys) key.m_onKeyCollect += OnKeyCollect;
        }

        //Ensure all colliders attached to the gameobject are enabled
        else
        {
            //Ensure all colliders attached to the gameobject are enabled
            foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.transform.tag != "Player") return;

        GameMode.m_instance.CompleteLevel();
    }

    void OnKeyCollect(Key _key)
    {
        m_keys.Remove(_key);

        //Open goal when all keys are collected
        if (m_keys.Count > 0) return;

        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = true;
        m_unlockEvent?.Invoke();

    }
}
