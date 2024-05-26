using UnityEngine.SceneManagement;
using UnityEngine;
using GMatch3.Scripts;

namespace GMatch3.UI
{
    public class LevelButton : MonoBehaviour
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
            InputEvents.LoadLevelEvent += OnLoadLevelEvent;
        }

        private void OnLoadLevelEvent(int scene)
        {        
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + scene);
        }
        private void UnregisterEvents()
        {
            InputEvents.LoadLevelEvent -= OnLoadLevelEvent;
        }
    }
}
