using System;
using Manager;
using ScriptableObjects.PowerUps;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Player
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField] 
        private bool _pickRandomPowerUpAtStart;
        
        [SerializeField] 
        private PowerUp _powerUpSO;
        
        [SerializeField] 
        private PowerUp[] _availablePowerUp;

        [SerializeField]
        private float _chargeDistance;

        private PowerUp _powerUpAssigned;
        
        public UnityAction OnPowerUpCharged;
        public UnityAction OnPowerUpUsed;

        private bool _isCharged;
        private float _currentDistance;
        private Vector3 _previousPos;

        private void Awake()
        {
            AssignPowerUp();
        }

        private void Start()
        {
            GameplayManager.Instance.OnRoundStart += Initialize;
            GameplayManager.Instance.OnRoundOver += Disable;
            RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished;
            Disable();
        }

        private void AssignPowerUp()
        {
            if (_pickRandomPowerUpAtStart)
            {
                var randomIndex = Random.Range(0, _availablePowerUp.Length);
                _powerUpAssigned = _availablePowerUp[randomIndex];
            }

            else
            {
                _powerUpAssigned = _powerUpSO;
            }
            
            Debug.Log($"Power up assigned: {_powerUpAssigned.PowerUpName}");
        }

        public void ResetLastPos()
        {
            _previousPos = transform.position;
        }

        private void OnPlayerTurnFinished(GameObject _player)
        {
            if (_player != gameObject) return;
            
            Disable();
        }

        private void Disable()
        {
            enabled = false;
        }

        private void Initialize()
        {
            enabled = true;
            StartChargingPowerUp();
        }

        private void FixedUpdate()
        {
            if (_isCharged) return;

            var currentPos = transform.position;
            _currentDistance += Vector3.Distance(_previousPos, currentPos);
            _previousPos = currentPos;
            if (_currentDistance >= _chargeDistance)
            {
                PowerUpCharged();
            }
        }

        private void PowerUpCharged()
        {
            _isCharged = true;
            OnPowerUpCharged?.Invoke();
        }

        [ContextMenu("Trigger power up")]
        public void TriggerPowerUp()
        {
            PowerUpAssigned.UsePowerUp(transform);
            OnPowerUpUsed?.Invoke();

            StartChargingPowerUp();
        }

        private void StartChargingPowerUp()
        {
            _isCharged = false;
            _currentDistance = 0;
            ResetLastPos();
        }

        public PowerUp PowerUpAssigned => _powerUpAssigned;

        public float ChargeRatio => _currentDistance / _chargeDistance;

        public bool IsCharged => _isCharged;
    }
}