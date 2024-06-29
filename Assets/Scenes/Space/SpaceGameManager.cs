using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame
{
    public class SpaceGameManager : MonoBehaviour
    {
        [SerializeField] int m_score; public int m_Score
        {
            get { return m_score; }
            set { m_score = value; }
        }
        int m_scorePerPlanet; public int m_ScorePerPlanet { get { return m_ScorePerPlanet; } }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
