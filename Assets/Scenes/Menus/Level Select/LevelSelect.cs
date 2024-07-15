using UnityEngine;
using UnityEngine.UI;
using UI.Pagination;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelSelect : MonoBehaviour
{
    public PagedRect m_pagedRect;

#if UNITY_EDITOR
    public GameManager m_gameManagerPrefab;
    public Button m_levelButtonPrefab;
    public ChangeScene.ChangeSceneProperties m_levelTransition;

    public void UpdatePagedRectPages()
    {
        m_pagedRect.UpdatePagination();

        //Add or remove pages to match the desired number of senarios
        while (m_pagedRect.Pages.Count < m_gameManagerPrefab.m_Scenarios.Length)
        {
            m_pagedRect.AddPageUsingTemplate();
        }
        while (m_pagedRect.Pages.Count > m_gameManagerPrefab.m_Scenarios.Length)
        {
            m_pagedRect.RemovePage(m_pagedRect.Pages.Count - 1);
        }

        // Update the content of each page to match the scenarios
        for (int pageIndex = 0; pageIndex < m_pagedRect.Pages.Count; pageIndex++)
        {
            GameManager.Senario senario = m_gameManagerPrefab.m_Scenarios[pageIndex];
            Page page = m_pagedRect.Pages[pageIndex];
            LevelSelectPage pageSelect = page.GetComponent<LevelSelectPage>();

            //Destroy level button
            while (pageSelect.m_levelButtonGroup.transform.childCount > 0) { DestroyImmediate(pageSelect.m_levelButtonGroup.transform.GetChild(0).gameObject); }

            //Change Page Title
            page.PageTitle = "World " + (pageIndex + 1).ToString();

            //Create level buttons
            for (int levelIndex = 0; levelIndex < senario.m_levels.Length; levelIndex++)
            {
                Button levelButton = Instantiate(m_levelButtonPrefab, pageSelect.m_levelButtonGroup.transform);
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
            GameManager.Senario senario = gameManager.m_Scenarios[pageIndex];
            Page page = m_pagedRect.Pages[pageIndex];
            LevelSelectPage pageSelect = page.GetComponent<LevelSelectPage>();

            //Get level buttons from button group children
            for (int levelIndex = 0; levelIndex < senario.m_levels.Length; levelIndex++)
            {
                int capturedIndex = levelIndex;
                pageSelect.m_levelButtonGroup.transform.GetChild(levelIndex).
                    GetComponent<Button>().onClick.
                    AddListener(() => { ChangeScene.LoadScene(senario.m_levels[capturedIndex], m_levelTransition); });
            }

            //Set hub world button onclic
            pageSelect.m_hubWorldButton.onClick.AddListener(() => { ChangeScene.LoadScene(senario.m_hubWorld, m_levelTransition); });
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