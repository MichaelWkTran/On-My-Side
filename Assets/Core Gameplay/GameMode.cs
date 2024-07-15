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
        m_isPlaying = false;
        m_winUI.m_time.text = Math.Round(m_time, 2).ToString();
        m_winUI.m_canvas.gameObject.SetActive(true);
    }
    
    public void GameOver()
    {
        m_isPlaying = false;
        m_gameOverUI.m_canvas.gameObject.SetActive(true);
    }

    public void Pause() { m_Paused = true; }

    public void UnPause() { m_Paused = false; }


    public void RetryLevel() { ChangeScene.LoadScene(SceneManager.GetActiveScene().name, m_restartLevelTransition); }

    public void GoToLevelSelect() { ChangeScene.LoadScene(m_levelSelectScene, m_levelSelectTransition); }

    public void GoToNextLevel()
    {
        GameManager.Senario[] senarios = GameManager.m_Instance.m_Scenarios;
        int senarioIndex; //The index of the senario to go to
        int levelIndex; //The index of the level to go to

        //Assign the senario and level index with the value associated with this scene
        {
            string thisSceneName = SceneManager.GetActiveScene().name;
            int[] senarioLevel = GameManager.m_Instance.GetLevelIndex(thisSceneName);
            senarioIndex = senarioLevel[0];
            levelIndex = senarioLevel[1];
        }

        //Update Level Index
        levelIndex++;

        //Check whether the next level is not available in this senario
        if (levelIndex >= senarios[senarioIndex].m_levels.Length)
        {
            //Move to next senario if all levels in this senario has finished
            senarioIndex++;
            levelIndex = 0;
        }

        //Check whether there are any more senarios to go to
        if (senarioIndex <= senarios.Length)
        {
            //Go to the new level scene
            ChangeScene.LoadScene(senarios[senarioIndex].m_levels[levelIndex], m_nextLevelTransition);
        }
        //If there are no more senarios
        else
        {
            //Go to the thanks for playing scene

        }
    }
}
