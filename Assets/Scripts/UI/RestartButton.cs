using GMatch3.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMatch3.UI
{
    public class RestartButton : MonoBehaviour
    {
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
            InputEvents.RestartLevelEvent += OnRestartLevelEvent;
        }

        private void OnRestartLevelEvent()
        {
            GameManager.Instance.ResetState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        private void UnregisterEvents()
        {
            InputEvents.RestartLevelEvent -= OnRestartLevelEvent;
        }
    }
}
