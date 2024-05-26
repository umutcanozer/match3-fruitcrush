using GMatch3.Scripts;
using GMatch3.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletedButton : MonoBehaviour
{
    private const int NextScene = 1;
    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        InputEvents.CompletedLevelEvent += OnCompletedLevelEvent;
    }

    private void OnCompletedLevelEvent()
    {
        GameManager.Instance.ResetState();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + NextScene;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1; 
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void UnregisterEvents()
    {
        InputEvents.CompletedLevelEvent -= OnCompletedLevelEvent;
    }
}
