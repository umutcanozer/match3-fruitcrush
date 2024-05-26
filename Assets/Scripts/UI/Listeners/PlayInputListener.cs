using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GMatch3.UI.Listeners
{
    public class PlayInputListener : MonoBehaviour
    {
        [SerializeField] Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            InputEvents.PlayEvent?.Invoke();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
