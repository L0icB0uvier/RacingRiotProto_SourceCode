using System;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        [SerializeField] 
        private int _numberOfRound;

        [SerializeField] 
        private float _delayBeforeRoundEnd = 2f;
        
        private int _currentRound;

        public enum EGameStates
        {
            Initialize,
            PrepareRound,
            RoundStartCountdown,
            Playing,
            RoundOver,
            GameOver,
        }

        private EGameStates _currentState = EGameStates.Initialize;

        public UnityAction OnInitialize;
        public UnityAction OnPrepareRound;
        public UnityAction OnRoundStartCountdown;
        public UnityAction OnRoundStart;
        public UnityAction OnRoundOver;
        public UnityAction OnGameOver;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentState = EGameStates.PrepareRound;
            _currentRound = 1;
        }

        public void ChangeState(EGameStates _newState)
        {
            _currentState = _newState;
            switch (_currentState)
            {
                case EGameStates.Initialize:
                    OnInitialize?.Invoke();
                    ChangeState(EGameStates.PrepareRound);
                    break;
                case EGameStates.PrepareRound:
                    OnPrepareRound?.Invoke();
                    break;
                case EGameStates.RoundStartCountdown:
                    OnRoundStartCountdown?.Invoke();
                    break;
                case EGameStates.Playing:
                    OnRoundStart?.Invoke();
                    break;
                case EGameStates.RoundOver:
                    Invoke(nameof(InvokeRoundEnd), _delayBeforeRoundEnd);
                    break;
                case EGameStates.GameOver:
                    OnGameOver?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InvokeRoundEnd()
        {
            OnRoundOver?.Invoke();
        }

        public void RoundComplete()
        {
            if (_currentRound == _numberOfRound)
            {
                GameOver();
                return;
            }
            _currentRound++;
            ChangeState(EGameStates.PrepareRound);
        }

        private void GameOver()
        {
            ChangeState(EGameStates.GameOver);
        }

        public void PlayAgain()
        {
            _currentRound = 1;
            ChangeState(EGameStates.Initialize);
        }
        
        public EGameStates CurrentState => _currentState;
        public int NumberOfRound => _numberOfRound;
        public int CurrentRound => _currentRound;
    }
}
