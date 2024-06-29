using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerCollider[] m_playerColliders; //List of colliders used to trigger player collision events

    void Awake()
    {
        //Collect all child player colliders
        m_playerColliders = GetComponentsInChildren<PlayerCollider>();
    }

    public void Kill()
    {

    }
}
