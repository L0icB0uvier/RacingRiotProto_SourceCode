using System;
using Manager;
using UnityEngine;

namespace Player
{
    public class RigidbodyValueSetter : MonoBehaviour
    {
        [SerializeField] private float _defaultDrag = .1f;
        [SerializeField] private float _loseControlDrag = 1f;
        
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Start()
        {
            GameplayManager.Instance.OnPrepareRound += ResetRigidbodyToDefault;
            RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished;
        }

        private void OnPlayerTurnFinished(GameObject _playerGo)
        {
            if (_playerGo != gameObject) return;
            _rb.drag = _loseControlDrag;
        }

        private void ResetRigidbodyToDefault()
        {
            _rb.drag = _defaultDrag;
        }
    }
}