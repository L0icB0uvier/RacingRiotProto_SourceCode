using System;
using Manager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RoundTimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        
        private void Start()
        {
            RoundTimer.Instance.OnTimerStart += OnTimerStart;
            RoundTimer.Instance.OnTimerStop += OnTimerStop;
            Hide();
        }
        
        private void Update()
        {
            UpdateTimerText();
        }
        
        private void UpdateTimerText()
        {
            _timerText.SetText(TimeSpan.FromSeconds(RoundTimer.Instance.CurrentTimer).ToString(@"mm\:ss\:ff"));
        }
        
        private void OnTimerStart()
        {
            UpdateTimerText();
            Show();
        }
        
        private void OnTimerStop()
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