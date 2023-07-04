using Player;
using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "Explosive", menuName = "ScriptableObjects/PowerUp/Explosive", order = 0)]
    public class Explosive : PowerUp
    {
        [SerializeField][Range(1, 3f)]
        private float _pushbackStrengthMultiplier;
        
        [SerializeField]
        private float _modifierDuration;

        [SerializeField][Range(.5f, 5f)][Tooltip("Ai will use this power up when the detect another player at this distance in front of them")]
        private float _aiUseDistanceBeforeImpact;
        
        [SerializeField][Range(.5f, 20f)][Tooltip("Ai will not use this Power up under this speed.")]
        private float _aiUseMinSpeed;

        [SerializeField] private LayerMask _playerLayerMask;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            _userTransform.GetComponent<CollisionPushback>().ApplyPushbackStrengthModifierForDuration(_pushbackStrengthMultiplier, _modifierDuration);
            Debug.Log("Explosive Power up used.");
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            if (_userRigidbody.velocity.sqrMagnitude < _aiUseMinSpeed * _aiUseMinSpeed) return false;
           
            var dir = _userRigidbody.velocity.normalized;
            return Physics.SphereCast(_userRigidbody.position, .5f, dir, out var hit, _aiUseDistanceBeforeImpact,
                _playerLayerMask);
        }
    }
}