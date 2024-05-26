using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

namespace GMatch3.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        public enum GameState
        {
            Playing,
            GameOver
        }

        private GameState _currentState = GameState.Playing;

        public GameState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
  
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Update()
        {
            Debug.Log(CurrentState);
        }

        public void GameOver()
        {
            if (_currentState != GameState.GameOver)
            {
                _currentState = GameState.GameOver;
            }
        }
        public void ResetState()
        {
            _currentState = GameState.Playing;
        }
    }
}
