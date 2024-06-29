using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame
{
    public class Planet : MonoBehaviour
    {
        public bool m_isFlagged;

        [SerializeField] float m_minRadius;
        [SerializeField] float m_maxRadius;
        [SerializeField] float m_minAngularVelocity;
        [SerializeField] float m_maxAngularVelocity;
        [SerializeField] float m_baseAngularVelocity;
        [SerializeField] Sprite[] m_planetSprites;
        [SerializeField] SpriteRenderer m_spriteRenderer;
        [SerializeField] Rigidbody2D m_rigidbody;

        void Start()
        {
            m_spriteRenderer.sprite = m_planetSprites[Random.Range(0, m_planetSprites.Length)];
            m_baseAngularVelocity = Random.Range(m_minAngularVelocity, m_maxAngularVelocity);
            transform.localScale = Vector3.one * Random.Range(m_minRadius, m_maxRadius);
        }

        void Update()
        {
            if (Mathf.Abs(m_rigidbody.angularVelocity) < Mathf.Abs(m_baseAngularVelocity)) m_rigidbody.angularVelocity = m_baseAngularVelocity;
        }
    }
}