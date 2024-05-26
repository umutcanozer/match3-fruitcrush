using GMatch3.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GMatch3
{
    public class MenuInputListener : MonoBehaviour
    {
        [SerializeField] Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            InputEvents.MenuEvent?.Invoke();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
