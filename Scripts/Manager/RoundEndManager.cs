using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class RoundEndManager : MonoBehaviour
    {
        public static RoundEndManager Instance;

        public UnityAction<GameObject> OnPlayerTurnFinished;

        private HashSet<GameObject> _playersFinished = new HashSet<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameplayManager.Instance.OnPrepareRound += OnPrepareGame;
        }

        private void OnPrepareGame()
        {
            _playersFinished.Clear();
        }

        public void PlayerTurnFinished(GameObject _playerFinished)
        {
            _playersFinished.Add(_playerFinished);

            OnPlayerTurnFinished?.Invoke(_playerFinished);
            
            if (_playersFinished.Count == PlayerManager.Instance.Players.Length)
            {
                GameplayManager.Instance.ChangeState(GameplayManager.EGameStates.RoundOver);
            }
        }
    }
}