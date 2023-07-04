using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "LiquidCooling", menuName = "ScriptableObjects/PowerUp/LiquidCooling", order = 0)]
    public class LiquidCooling : PowerUp
    {
        [SerializeField][Range(0f, 1f)]
        private float _cooldownModifier;
        
        [SerializeField] 
        private float _duration;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            var userPlayerStroke = _userTransform.GetComponent<PlayerStroke>();
            userPlayerStroke.ChangeCooldownMultiplierForDuration(_cooldownModifier, _duration);
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            return true;
        }
    }
}