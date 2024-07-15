using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour 
{
    [Serializable] public struct ChangeSceneProperties
    {
        public SceneTransition m_sceneTransitionPrefab;
        public LoadSceneMode m_loadSceneMode;
        public bool m_loadedAsync;
    } public ChangeSceneProperties m_changeSceneProperties;

    public static void LoadScene(string _sceneName, ChangeSceneProperties _changeSceneProperties)
    {
        if (_changeSceneProperties.m_sceneTransitionPrefab == null)
        {
            if (!_changeSceneProperties.m_loadedAsync) SceneManager.LoadScene(_sceneName, _changeSceneProperties.m_loadSceneMode);
            else SceneManager.LoadSceneAsync(_sceneName, _changeSceneProperties.m_loadSceneMode);
        }
        else
        {
            SceneTransition sceneTransition = Instantiate(_changeSceneProperties.m_sceneTransitionPrefab);
            sceneTransition.m_changeSceneProperties = _changeSceneProperties;
            sceneTransition.m_newSceneName = _sceneName;
        }
    }

    public void LoadScene(string _sceneName) { LoadScene(_sceneName, m_changeSceneProperties); }
}