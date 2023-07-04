using Player;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(PowerUpController))][RequireComponent(typeof(Rigidbody))]
    public class PowerUpAIController : MonoBehaviour
    {
        private PowerUpController _powerUpController;
        private Rigidbody _rb;

        private void Awake()
        {
            _powerUpController = GetComponent<PowerUpController>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_powerUpController.IsCharged == false) return;

            if (_powerUpController.PowerUpAssigned.AreUseConditionMet(_rb))
            {
                _powerUpController.TriggerPowerUp();
            }
        }
    }
}