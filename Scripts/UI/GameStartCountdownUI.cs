using System;
using Manager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountdownUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countdownText;
        
        private void Start()
        {
            GameStartCountdown.Instance.OnCountdownStart += OnCountdownStart;
            GameStartCountdown.Instance.OnCountdownEnd += OnCountdownEnd;
            
            Hide();
        }

        private void Update()
        {
            UpdateCountdownText();
        }

        private void UpdateCountdownText()
        {
            _countdownText.SetText(Mathf.CeilToInt(GameStartCountdown.Instance.CurrentCountdownValue).ToString());
        }

        private void OnCountdownStart()
        {
            UpdateCountdownText();
            Show();
        }
        
        private void OnCountdownEnd()
        {
            Hide();
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