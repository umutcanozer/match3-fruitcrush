using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMatch3.UI
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Image _scoreBar;
        [SerializeField] private int _scoreGoal;

        public int Score { get; private set; } = 0;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            UpdateScoreText();
            UpdateBar();
        }
        public void AddScore(int point)
        {
            Score += point;
        }
        private void UpdateScoreText()
        {
            _scoreText.text = "Score: " + Score;
        }
        private void UpdateBar()
        {
            _scoreBar.fillAmount = (float)Score / (float)_scoreGoal;
        }
    }
}
