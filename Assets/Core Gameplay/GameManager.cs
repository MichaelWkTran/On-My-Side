using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    static GameManager m_instance;
    public static GameManager m_Instance
    {
        get
        {
            #if UNITY_EDITOR
            if (m_instance == null) { m_instance = (GameManager)Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Core Gameplay/GameManager.prefab", typeof(GameManager)));             }
            #endif

            return m_instance;
        }
    }

    [Serializable] public struct Senario
    {
        public string m_hubWorld;
        public string[] m_levels;
    }
    [SerializeField] Senario[] m_senarios;
    public Senario[] m_Scenarios { get { return m_senarios; } }
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (m_instance == null) m_instance = this;
        else if (m_instance != this) Destroy(gameObject);
    }

    #region Levels
    public int[] GetLevelIndex(string _levelName)
    {
        for (int senarioIndex = 0; senarioIndex < m_Scenarios.Length; senarioIndex++)
        {
            //Get searched senario
            Senario senario = m_Scenarios[senarioIndex];
            
            //Find the level stored in the senario
            int foundLevelIndex = Array.IndexOf(senario.m_levels, _levelName);

            //Check whether the level is found and skip this interation if not
            if (foundLevelIndex < 0) continue;

            //Return the found senario and level
            return new int[] { senarioIndex, foundLevelIndex };
        }

        return null;
    }

    #endregion
}
