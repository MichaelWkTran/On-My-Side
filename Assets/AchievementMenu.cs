using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenu : MonoBehaviour
{
    AchievementSystem m_achievementSystem;
    [SerializeField] AchievementUI m_achivementUIPrefab;
    [SerializeField] LayoutGroup m_achievementGroup;

    // Start is called before the first frame update
    void Start()
    {
        m_achievementSystem = FindObjectOfType<AchievementSystem>();
        foreach (AchievementSystem.Achievement achievement in m_achievementSystem.m_achievements)
        {
            AchievementUI achievementUI = Instantiate(m_achivementUIPrefab, m_achievementGroup.transform);
            achievementUI.m_achievement = achievement;
            achievementUI.m_icon.sprite = achievement.m_icon;
            achievementUI.m_nameText.text = achievement.m_name;
            achievementUI.m_descriptionText.text = achievement.m_description;
            achievementUI.m_progressSlider.maxValue = achievement.m_maxProgress;
            achievementUI.m_progressSlider.value = achievement.m_progress;
            achievement.m_onUpdateNotify += () => achievementUI.UpdateUI();
        }

    }
}
