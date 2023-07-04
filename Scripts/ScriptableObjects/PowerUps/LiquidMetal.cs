using Player;
using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "LiquidMetal", menuName = "ScriptableObjects/PowerUp/LiquidMetal", order = 0)]
    public class LiquidMetal : PowerUp
    {
        [SerializeField][Range(2f, 20f)]
        private float _pushbackResistanceMultiplier;
        
        [SerializeField] 
        private float _modifierDuration;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            _userTransform.GetComponent<CollisionPushback>().ApplyPushbackResistanceModifierForDuration(_pushbackResistanceMultiplier, _modifierDuration);
            Debug.Log("Liquid metal Power up used.");
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            return true;
        }
    }
}