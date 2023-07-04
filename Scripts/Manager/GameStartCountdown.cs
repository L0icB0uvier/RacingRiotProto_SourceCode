using System;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class GameStartCountdown : MonoBehaviour
    {
        public static GameStartCountdown Instance;
        
        [SerializeField] private int _countDownStartValue;
        private float _currentCountdownValue;

        public UnityAction OnCountdownStart;
        public UnityAction OnCountdownEnd;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameplayManager.Instance.OnRoundStartCountdown += OnGameStateChanged;
            StopCountdown();
        }

        private void OnGameStateChanged()
        {
            InitializeCountdown();
        }

        private void Update()
        {
            _currentCountdownValue -= Time.deltaTime;
            if (_currentCountdownValue <= 0)
            {
                TriggerGameStart();
            }
        }

        private void TriggerGameStart()
        {
            GameplayManager.Instance.ChangeState(GameplayManager.EGameStates.Playing);
            OnCountdownEnd?.Invoke();
            StopCountdown();
        }

        private void InitializeCountdown()
        {
            _currentCountdownValue = _countDownStartValue;
            OnCountdownStart?.Invoke();
            StartCountdown();
        }

        private void StartCountdown()
        {
            enabled = true;
        }

        private void StopCountdown()
        {
            enabled = false;
        }

        public float CurrentCountdownValue => _currentCountdownValue;
    }
}