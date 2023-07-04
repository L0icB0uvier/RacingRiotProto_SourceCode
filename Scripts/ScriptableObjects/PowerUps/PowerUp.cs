using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    public abstract class PowerUp : ScriptableObject
    {
        [SerializeField] 
        private string _powerUpName;
        
        [SerializeField] 
        private string _description;
        
        public abstract void UsePowerUp(Transform _userTransform);

        public abstract bool AreUseConditionMet(Rigidbody _userRigidbody);

        public string PowerUpName => _powerUpName;

        public string Description => _description;
    }
}