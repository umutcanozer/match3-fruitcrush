using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GMatch3.UI.Listeners
{
    public class LoadInputListener : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] private int _nextScene;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            InputEvents.LoadLevelEvent?.Invoke(_nextScene);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
