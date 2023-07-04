using System;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class RoundTimer : MonoBehaviour
    {
        public static RoundTimer Instance;
        
        [SerializeField] private int _timerDuration;

        [SerializeField] private ETimerType _timerType;
        
        private float _currentTimer;

        public UnityAction OnTimerStart;
        public UnityAction OnTimerStop;
        
        private enum ETimerType
        {
            Increment,
            Decrement
        }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameplayManager.Instance.OnRoundStart += InitializeTimer;
            GameplayManager.Instance.OnRoundOver += StopTimer;
            StopTimer();
        }

        private void Update()
        {
            switch (_timerType)
            {
                case ETimerType.Increment:
                    _currentTimer += Time.deltaTime;
                    if (_currentTimer >= _timerDuration)
                    {
                        TriggerGameOver();
                    }
                    break;
                case ETimerType.Decrement:
                    _currentTimer -= Time.deltaTime;
                    if (_currentTimer <= 0)
                    {
                        TriggerGameOver();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void TriggerGameOver()
        {
            GameplayManager.Instance.ChangeState(GameplayManager.EGameStates.RoundOver);
            StopTimer();
        }

        private void InitializeTimer()
        {
            _currentTimer = _timerType == ETimerType.Increment? 0 : _timerDuration;
            enabled = true;
            OnTimerStart?.Invoke();
        }

        private void StopTimer()
        {
            _currentTimer = _timerType == ETimerType.Increment ? _timerDuration : 0;
            enabled = false;
            OnTimerStop?.Invoke();
        }

        public float CurrentTimer => _currentTimer;
    }
}