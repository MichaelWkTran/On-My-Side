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

    void Awake()
    {
        //Check singleton
        DontDestroyOnLoad(gameObject);
        if (m_instance == null) m_instance = this;
        else if (m_instance != this) Destroy(gameObject);

        //Load data from save system
        {
            //Load scenario and level data save data
            {
                ScenariosSave scenariosSave = JsonUtility.FromJson<ScenariosSave>(SaveSystem.m_Data.m_data[(int)SaveSystem.SaveIDs.Levels]);
                for (int i = 0; i < m_scenarios.Length; i++) m_scenarios[i].LoadData(scenariosSave.m_scenariosData[i]);
            }
        }
    }

    void OnDestroy()
    {
        //Skip event if not singleton instance
        if (m_instance != this) return;

        //Create Data to be saved in the Save System
        {
            //Save scenario and level save data
            SaveSystem.m_Data.m_data[(int)SaveSystem.SaveIDs.Levels] = JsonUtility.ToJson(new ScenariosSave(m_scenarios));
            
        }

        //Save Data
        SaveSystem.Save();
    }

    #region Levels
    [Serializable] public struct Scenario
    {
        [Serializable] public struct Level
        {
            public string m_levelScene;
            public uint m_collectedStars;
            public float m_time;
        }
        public string m_hubWorld;
        public Level[] m_levels;

        public void LoadData(ScenariosSave.ScenarioSave _scenarioSaveData)
        {
            for (int i = 0; i < Mathf.Min(m_levels.Length, _scenarioSaveData.m_levelData.Length); i++)
            {
                m_levels[i].m_collectedStars = _scenarioSaveData.m_levelData[i].m_collectedStars;
                m_levels[i].m_time = _scenarioSaveData.m_levelData[i].m_time;
            }
        }
    }

    [Serializable] public struct ScenariosSave
    {
        [Serializable] public struct ScenarioSave
        {
            [Serializable] public struct LevelSave
            {
                public uint m_collectedStars;
                public float m_time;
            } public LevelSave[] m_levelData;
            public void SaveData(Scenario _scenario)
            {
                m_levelData = new LevelSave[_scenario.m_levels.Length];
                for (int i = 0; i < m_levelData.Length; i++)
                {
                    m_levelData[i].m_collectedStars = _scenario.m_levels[i].m_collectedStars;
                    m_levelData[i].m_time = _scenario.m_levels[i].m_time;
                }
            }
        } public ScenarioSave[] m_scenariosData;

        public ScenariosSave(Scenario[] _scenarios)
        {
            m_scenariosData = new ScenarioSave[_scenarios.Length];
            for (int i = 0; i < _scenarios.Length; i++) m_scenariosData[i].SaveData(_scenarios[i]);
        }
    }


    public Scenario[] m_scenarios;

    public int[] GetLevelIndex(string _levelName)
    {
        for (int scenarioIndex = 0; scenarioIndex < m_scenarios.Length; scenarioIndex++)
        {
            //Get searched scenario
            Scenario scenario = m_scenarios[scenarioIndex];

            //Find the level stored in the scenario
            int foundLevelIndex = Array.FindIndex(scenario.m_levels, i => i.m_levelScene == _levelName);

            //Check whether the level is found and skip this interation if not
            if (foundLevelIndex < 0) continue;

            //Return the found scenario and level
            return new int[] { scenarioIndex, foundLevelIndex };
        }

        return null;
    }

    #endregion
}
