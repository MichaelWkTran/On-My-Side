using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    [HideInInspector] public AchievementSystem.Achievement m_achievement;
    public Image m_icon;
    public TMP_Text m_nameText;
    public TMP_Text m_descriptionText;
    public Slider m_progressSlider;

    public void UpdateUI()
    {
        m_progressSlider.value = m_achievement.m_progress;
    }
}
