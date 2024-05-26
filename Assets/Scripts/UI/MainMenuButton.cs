using GMatch3.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMatch3
{
    public class MainMenuButton : MonoBehaviour
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
            InputEvents.MenuEvent += OnMenuEvent;
        }

        private void OnMenuEvent()
        {
            SceneManager.LoadScene(0);
        }
        private void UnregisterEvents()
        {
            InputEvents.MenuEvent -= OnMenuEvent;
        }
    }
}
