using GMatch3.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace GMatch3
{
    public class EndGame : MonoBehaviour
    {
        //level3 12 kýrmýzý 8 mavi 21
        //level2 8 kýrmýzý 2 yeþil 23
        //level1 5 sarý 5 kýrmýzý 02

        [System.Serializable]
        public struct FruitIndex
        {
            public int RequiredFruit;
            public int CrushedFruit;
        };
        [SerializeField] private List<int> _indicesToCheck;

        public FruitIndex[] piecePrefabs;

        [SerializeField] private float _timer;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private List<TextMeshProUGUI> _requiredTexts;
        [SerializeField] private GameObject _restartUI;
        [SerializeField] private GameObject _levelUI;

        bool _allIndicesCompleted = false;

        private void Awake()
        {
            _restartUI.SetActive(false);
            _levelUI.SetActive(false);
        }
        private void Update()
        {
            if (_timer <= 0) GameOver();
            CopyListToStruct();
            CheckIndices();
            UpdateTimer();
            DecreaseFruit();

        }

        private void DecreaseFruit()
        {
            for (int i = 0; i < _indicesToCheck.Count; i++)
            {
                int index = _indicesToCheck[i];
                int numberOfFruits = piecePrefabs[index].RequiredFruit - piecePrefabs[index].CrushedFruit;
                Debug.Log(numberOfFruits);
                int numberOfCrushed = numberOfFruits < 0 ? 0 : numberOfFruits;
                _requiredTexts[i].text = numberOfCrushed.ToString();
            }
        }

        private void UpdateTimer()
        {
            if (_allIndicesCompleted)
            {
                LevelCompleted();
                return;
            }
            _timer -= Time.deltaTime;
            _timer = _timer < 0 ? 0 : _timer;
            _timerText.text = ((int)_timer).ToString();
        }

        private void CopyListToStruct()
        {
            for (int i = 0; i < piecePrefabs.Length; i++)
            {
                piecePrefabs[i].CrushedFruit = GridManager.instance.CrushedFruits[i];
            }
        }

        private void CheckIndices()
        {
            
            foreach (int index in _indicesToCheck)
            {
                if (index >= 0 && index < piecePrefabs.Length)
                    if (piecePrefabs[index].RequiredFruit >= piecePrefabs[index].CrushedFruit)
                    {
                    _allIndicesCompleted = false;
                    break;
                    }
                    else { _allIndicesCompleted = true; }
                
                //if (_allIndicesCompleted)
                //{
                //    Debug.Log("All specified indices have CrushedFruit values greater than or equal to RequiredFruit.");
                //}
            }
        }

        private void GameOver()
        {
            GameManager.Instance.GameOver();
            _restartUI.SetActive(true);
        }

        private void LevelCompleted()
        {
            GameManager.Instance.GameOver();
            _levelUI.SetActive(true);
        }
    }
}
