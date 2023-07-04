using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Button _playAgainButton;
        
        private void Start()
        {
            GameplayManager.Instance.OnGameOver += Show;
            GameplayManager.Instance.OnPrepareRound += Hide;
            _playAgainButton.onClick.AddListener(PlayAgainButtonClicked);
            Hide();
        }

        private void PlayAgainButtonClicked()
        {
            GameplayManager.Instance.PlayAgain();
            Hide();
        }

        private void Initialize()
        {
            Show();
        }
        
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}