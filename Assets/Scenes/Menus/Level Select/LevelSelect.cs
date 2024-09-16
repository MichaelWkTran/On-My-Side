using UnityEngine;
using UnityEngine.UI;
using UI.Pagination;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelSelectNamespace
{
    public class LevelSelect : MonoBehaviour
    {
        public PagedRect m_pagedRect;
        public ChangeScene.ChangeSceneProperties m_levelTransition;

#if UNITY_EDITOR
        public GameManager m_gameManagerPrefab;
        public LevelSelectLevelButton m_levelButtonPrefab;

        public void UpdatePagedRectPages()
        {
            m_pagedRect.UpdatePagination();

            //Add or remove pages to match the desired number of scenarios
            for (int i = m_pagedRect.Pages.Count; i < m_gameManagerPrefab.m_scenarios.Length; i++) m_pagedRect.AddPageUsingTemplate();
            for (int i = m_pagedRect.Pages.Count; i > m_gameManagerPrefab.m_scenarios.Length; i--) m_pagedRect.RemovePage(i - 1);

            // Update the content of each page to match the scenarios
            for (int pageIndex = 0; pageIndex < m_pagedRect.Pages.Count; pageIndex++)
            {
                GameManager.Scenario scenario = m_gameManagerPrefab.m_scenarios[pageIndex];
                Page page = m_pagedRect.Pages[pageIndex];
                LevelSelectPage pageSelect = page.GetComponent<LevelSelectPage>();

                //Destroy level button
                while (pageSelect.m_levelButtonGroup.transform.childCount > 0) { DestroyImmediate(pageSelect.m_levelButtonGroup.transform.GetChild(0).gameObject); }

                //Change Page Title
                page.PageTitle = "World " + (pageIndex + 1).ToString();

                //Create level buttons
                for (int levelIndex = 0; levelIndex < scenario.m_levels.Length; levelIndex++)
                {
                    Button levelButton = (Button)PrefabUtility.InstantiatePrefab(m_levelButtonPrefab, pageSelect.m_levelButtonGroup.transform);
                    levelButton.GetComponentInChildren<TMPro.TMP_Text>().text = (levelIndex + 1).ToString();
                }
            }
        }
#endif

        void Start()
        {
            //Get Game Manager
            GameManager gameManager = GameManager.m_Instance;

            //Set level button onclick events
            for (int pageIndex = 0; pageIndex < m_pagedRect.Pages.Count; pageIndex++)
            {
                GameManager.Scenario scenario = gameManager.m_scenarios[pageIndex];
                Page page = m_pagedRect.Pages[pageIndex];
                LevelSelectPage pageSelect = page.GetComponent<LevelSelectPage>();

                //Get level buttons from button group children
                for (int levelIndex = 0; levelIndex < scenario.m_levels.Length; levelIndex++)
                {
                    GameManager.Scenario.Level level = scenario.m_levels[levelIndex];
                    LevelSelectLevelButton levelSelectButton = pageSelect.m_levelButtonGroup.transform.GetChild(levelIndex).GetComponent<LevelSelectLevelButton>();

                    //Disable level buttons if not unlocked
                    if (levelIndex > 0 && scenario.m_levels[levelIndex-1].m_time <= 0)
                    {
                        levelSelectButton.GetComponent<Button>().interactable = false;
                        continue;
                    }

                    //Set onclick on level button
                    int capturedIndex = levelIndex;
                    levelSelectButton.GetComponent<Button>().onClick.
                        AddListener(() => { ChangeScene.LoadScene(scenario.m_levels[capturedIndex].m_levelScene, m_levelTransition); });

                    //Set button time
                    levelSelectButton.m_time.gameObject.SetActive(level.m_time > 0);
                    if (level.m_time > 0) levelSelectButton.m_time.text = "T: " + level.m_time.ToString();

                    //Set button stars
                    for (int i = 0; i < levelSelectButton.m_stars.Length; i++) levelSelectButton.m_stars[i].gameObject.SetActive(i + 1 <= level.m_collectedStars);
                }

                //Set hub world button onclic
                pageSelect.m_hubWorldButton.onClick.AddListener(() => { ChangeScene.LoadScene(scenario.m_hubWorld, m_levelTransition); });
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelSelect))]
    public class LevelSelectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Get the target script
            LevelSelect levelSelect = (LevelSelect)target;

            // Add a button to call UpdatePagedRectPages()
            if (GUILayout.Button("Update Paged Rect Pages")) levelSelect.UpdatePagedRectPages();
        }
    }
#endif
};