using GMatch3.Scripts;
using GMatch3.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMatch3.UI
{
    public class QuitButton : MonoBehaviour
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
            InputEvents.ExitEvent += OnExitEvent;
        }

        private void OnExitEvent()
        {
            Application.Quit();
        }
        private void UnregisterEvents()
        {
            InputEvents.ExitEvent -= OnExitEvent;
        }
    }
}
