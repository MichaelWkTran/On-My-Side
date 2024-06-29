using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollider : MonoBehaviour
{
    Player m_player;

    void Awake()
    {
        //Get the root player component
        m_player = GetComponentInParent<Player>(true);
    }

    void OnCollisionEnter2D(Collision2D _collision)
    {
        //Kill the player
        if (_collision.gameObject.tag == "Kill")
        {
            m_player.Kill();
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        //Kill the player
        if (_collision.gameObject.tag == "Kill")
        {
            m_player.Kill();
            return;
        }
    }
}
