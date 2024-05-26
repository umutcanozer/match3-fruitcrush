using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMatch3.UI
{
    public class PlayButton : MonoBehaviour
    {
        private const int nextScene = 1;

        private void OnEnable()
        {
            InputEvents.PlayEvent += OnPlayEvent;
        }

        private void OnPlayEvent()
        {
            Debug.Log("basarili");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + nextScene);

        }
        private void OnDisable()
        {
            InputEvents.PlayEvent -= OnPlayEvent;
        }
    }
}
