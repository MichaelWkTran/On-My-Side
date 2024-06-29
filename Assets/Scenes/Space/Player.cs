using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] SpriteRenderer m_spriteRenderer;
        [SerializeField] Sprite m_idle;
        [SerializeField] Sprite m_floating;
        [SerializeField] GameObject m_flagPrefab;

        void OnCollisionEnter2D(Collision2D _collision)
        {
            //Earn Points by touching a planet
            {
                Planet planet = _collision.gameObject.GetComponent<Planet>();
                if (planet && !planet.m_isFlagged)
                {
                    planet.m_isFlagged = true;

                    //Place Flag
                    GameObject flag = Instantiate(m_flagPrefab);
                    flag.transform.position = _collision.contacts[0].point;
                    flag.transform.rotation = Quaternion.FromToRotation(Vector2.zero, _collision.contacts[0].normal);
                    flag.transform.SetParent(planet.transform, true);

                    //Add Points
                    FindObjectOfType<SpaceGameManager>().m_Score += FindObjectOfType<SpaceGameManager>().m_ScorePerPlanet;
                }
            }

            //Die by colliding with an obstacle

        }
    }
}

