using System;
using Manager;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerTurnOverUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _playerTimerText;
        
        private void Start()
        {
            RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished;
            GameplayManager.Instance.OnRoundOver += Hide;
            Hide();
        }

        private void OnPlayerTurnFinished(GameObject _player)
        {
            if(_player != PlayerManager.Instance.OwnerPlayer) return;
            var playerTimer = _player.GetComponent<PlayerRoundPerformance>().Timer;
            _playerTimerText.SetText(TimeSpan.FromSeconds(playerTimer).ToString(@"mm\:ss\:ff")); 
            Show();
        }

        private void Initialize()
        {
            
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