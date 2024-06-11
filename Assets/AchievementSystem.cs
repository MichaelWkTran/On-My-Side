using System;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;

[RequireComponent(typeof(ScriptMachine))]
public class AchievementSystem : MonoBehaviour
{
    [Serializable] public class Achievement
    {
        public string m_name; //The name of the achievement
        [TextArea] public string m_description; //The critera that the achievement have to meet
        public Sprite m_icon; //The icon representing the achievement
        public float m_progress; //How much does the player need to do to complete the achievement.
        public float m_maxProgress; //The amount of progress that have to be met for the achievement to be completed.

        public string m_onInitEvent; //The visual scripting custom event used to initialize the achievement
        public string m_onUpdateEvent; //The visual scripting custom event used to update the achievement

        public Dictionary<Observer, Observer.ObserverDelegate> m_observers;
    }

    public Achievement[] m_achievements;

    void Start()
    {
        foreach (Achievement achievement in m_achievements) CustomEvent.Trigger(gameObject, achievement.m_onInitEvent, achievement);
        
    }

    public void AddAchievementObserver(Observer _observer,  Achievement _achievement)
    {
        //Create dictionary for given achievement
        if (_achievement.m_observers == null) _achievement.m_observers = new Dictionary<Observer, Observer.ObserverDelegate>();

        //Create delegate and add to achievement dictionary
        Observer.ObserverDelegate onNotify = () => CustomEvent.Trigger(gameObject, _achievement.m_onUpdateEvent, _achievement);
        _achievement.m_observers.Add(_observer, onNotify);

        //Add delegate to observer
        _observer.m_onNotify += onNotify;
    }

    public void CompleteAchievement(Achievement _achievement)
    {
        foreach (KeyValuePair<Observer, Observer.ObserverDelegate> pair in _achievement.m_observers) pair.Key.m_onNotify -= pair.Value;
        _achievement.m_observers = null;
        _achievement.m_progress = _achievement.m_maxProgress;
    }
}

#region Visual Scripting Custom Nodes
[CreateAssetMenu(menuName = "Custom Nodes/Expose Achievement")]
public class ExposeAchievement : Unit
{
    [DoNotSerialize] public ValueInput m_achievementInput;
    [DoNotSerialize] public ValueOutput m_progressOutput;
    [DoNotSerialize] public ValueOutput m_maxProgressOutput;

    protected override void Definition()
    {
        //Take in the achievement class and output the data
        m_achievementInput = ValueInput<AchievementSystem.Achievement>("Achievement");
        m_progressOutput = ValueOutput("Progression", (flow) => flow.GetValue<AchievementSystem.Achievement>(m_achievementInput).m_progress);
        m_maxProgressOutput = ValueOutput("Max Progression", (flow) => flow.GetValue<AchievementSystem.Achievement>(m_achievementInput).m_maxProgress);
    }
}

[CreateAssetMenu(menuName = "Custom Nodes/Update Achievement")]
public class UpdateAchievement : Unit
{
    [DoNotSerialize] public ControlInput m_inputTrigger;
    [DoNotSerialize] public ValueInput m_achievementInput;
    [DoNotSerialize] public ValueInput m_newProgressInput;
    
    [DoNotSerialize] public ControlOutput m_outputTrigger;
    [DoNotSerialize] public ValueOutput m_achievementOutput;

    [DoNotSerialize] AchievementSystem.Achievement m_resultingValue;

    protected override void Definition()
    {
        //Add Trigger Ports and Logic
        m_inputTrigger = ControlInput("", (flow) =>
        {
            m_resultingValue = flow.GetValue<AchievementSystem.Achievement>(m_achievementInput);
            m_resultingValue.m_progress = flow.GetValue<float>(m_newProgressInput);

            return m_outputTrigger;
        });
        m_outputTrigger = ControlOutput("");

        //Add Property Ports
        m_achievementInput = ValueInput<AchievementSystem.Achievement>("Achievement");
        m_newProgressInput = ValueInput<float>("New Progress");
        m_achievementOutput = ValueOutput("Resulting Achievement", (flow) => m_resultingValue);

        //Set node relations
        Requirement(m_achievementInput, m_inputTrigger);
        Requirement(m_newProgressInput, m_inputTrigger);
        Succession(m_inputTrigger, m_outputTrigger);
        Assignment(m_inputTrigger, m_achievementOutput);
    }
}
#endregion

