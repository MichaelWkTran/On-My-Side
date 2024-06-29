using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public static GameMode m_instance;
    [SerializeField] uint m_collectedStars = 0U;
    [SerializeField] float m_time = 0.0f;

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
        public TMP_Text m_time;
        public RectTransform[] m_starUI;

    } [SerializeField] GameOverUI m_gameOverUI;

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
        if (Time.deltaTime > 0)
        {
            m_time += Time.deltaTime;

        }
    }

    public void AddStar()
    {
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
        GoToNextLevel();
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void GoToLevelSelect()
    {

    }

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
            SceneManager.LoadScene(senarios[senarioIndex].m_levels[levelIndex]);
        }
        //If there are no more senarios
        else
        {
            //Go to the thanks for playing scene

        }
    }
}
