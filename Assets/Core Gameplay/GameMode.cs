using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public static GameMode m_instance;
    [SerializeField] uint m_collectedStars = 0U;
    [SerializeField] float m_time = 0.0f;
    bool m_paused; public bool m_Paused
    {
        get { return m_paused; }
        set
        {
            m_paused = value;
            if (m_paused) { Time.timeScale = 0.0f; m_pauseUI.m_canvas.gameObject.SetActive(true); }
            else { Time.timeScale = 1.0f; m_pauseUI.m_canvas.gameObject.SetActive(false); }
        }
    }
    bool m_isPlaying = true;

    [Serializable] struct GameplayUI
    {
        public Canvas m_canvas;
        public TMP_Text m_time;
        public RectTransform[] m_starUI;

    } [SerializeField] GameplayUI m_gameplayUI;
    [Serializable] struct PauseMenuUI
    {
        public Canvas m_canvas;

    } [SerializeField] PauseMenuUI m_pauseUI;
    [Serializable] struct WinUI
    {
        public Canvas m_canvas;
        public TMP_Text m_time;
        public RectTransform[] m_starUI;

    } [SerializeField] WinUI m_winUI;
    [Serializable] struct GameOverUI
    {
        public Canvas m_canvas;

    } [SerializeField] GameOverUI m_gameOverUI;
    [SerializeField] ChangeScene.ChangeSceneProperties m_restartLevelTransition;
    [SerializeField] string m_levelSelectScene;
    [SerializeField] ChangeScene.ChangeSceneProperties m_levelSelectTransition;
    [SerializeField] ChangeScene.ChangeSceneProperties m_nextLevelTransition;

    void Awake()
    {
        m_instance = this;
    }

    void Start()
    {
        //Init gameplay UI
        {
            //Hide Star UI
            foreach (RectTransform starUI in m_gameplayUI.m_starUI) starUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //Update time and UI
        if (Time.deltaTime > 0 && m_isPlaying)
        {
            m_time += Time.deltaTime;

        }
    }

    void OnDestroy()
    {
        //Reset the timescale to prevent the next scene from being paused
        Time.timeScale = 1.0f;
    }

    public void AddStar()
    {
        if (!m_isPlaying) return;

        //Get Stars
        RectTransform starUI = m_gameplayUI.m_starUI[m_collectedStars];
        
        //Show collected star
        starUI.gameObject.SetActive(true);
        
        //Animate the star
        starUI.localScale = Vector3.zero;
        LeanTween.scale(starUI.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutElastic);
        
        //Update the number of collected stars
        m_collectedStars++;
    }

    public void CompleteLevel()
    {
        //Pause Game and open win UI
        m_isPlaying = false;
        Time.timeScale = 0.0f;
        m_winUI.m_canvas.gameObject.SetActive(true);

        //Get and Update Save Variables
        int[] scenarioLevel = GameManager.m_Instance.GetLevelIndex(SceneManager.GetActiveScene().name);
        GameManager.Scenario.Level level = GameManager.m_Instance.m_scenarios[scenarioLevel[0]].m_levels[scenarioLevel[1]];
        m_time = (float)Math.Round(m_time, 2);
        
        //Update save data and trigger new best time
        if (m_time < level.m_time || level.m_time <= 0.0f)
        {
            level.m_time = m_time;
        }

        level.m_collectedStars = Math.Max(m_collectedStars, level.m_collectedStars);

        //Save Data
        GameManager.m_Instance.m_scenarios[scenarioLevel[0]].m_levels[scenarioLevel[1]] = level;

        //Update Win Time
        m_winUI.m_time.text = "T: " + m_time.ToString();

        //Update Win Time
        for (int i = 0; i < m_winUI.m_starUI.Length; i++) m_winUI.m_starUI[i].gameObject.SetActive(i + 1 <= m_collectedStars);
    }
    
    public void GameOver()
    {
        m_isPlaying = false;
        Time.timeScale = 0.0f;
        m_gameOverUI.m_canvas.gameObject.SetActive(true);
    }

    public void Pause() { m_Paused = true; }

    public void UnPause() { m_Paused = false; }

    public void RetryLevel() { ChangeScene.LoadScene(SceneManager.GetActiveScene().name, m_restartLevelTransition); }

    public void GoToLevelSelect() { ChangeScene.LoadScene(m_levelSelectScene, m_levelSelectTransition); }

    public void GoToNextLevel()
    {
        GameManager.Scenario[] scenarios = GameManager.m_Instance.m_scenarios;
        int scenarioIndex; //The index of the scenario to go to
        int levelIndex; //The index of the level to go to

        //Assign the scenario and level index with the value associated with this scene
        {
            string thisSceneName = SceneManager.GetActiveScene().name;
            int[] scenarioLevel = GameManager.m_Instance.GetLevelIndex(thisSceneName);
            scenarioIndex = scenarioLevel[0];
            levelIndex = scenarioLevel[1];
        }

        //Update Level Index
        levelIndex++;

        //Check whether the next level is not available in this scenario
        if (levelIndex >= scenarios[scenarioIndex].m_levels.Length)
        {
            //Move to next scenario if all levels in this scenario has finished
            scenarioIndex++;
            levelIndex = 0;
        }

        //Check whether there are any more scenarios to go to
        if (scenarioIndex <= scenarios.Length)
        {
            //Go to the new level scene
            ChangeScene.LoadScene(scenarios[scenarioIndex].m_levels[levelIndex].m_levelScene, m_nextLevelTransition);
        }
        //If there are no more scenarios
        else
        {
            //Go to the thanks for playing scene

        }
    }
}
