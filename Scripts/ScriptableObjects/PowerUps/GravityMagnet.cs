using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "GravityMagnet", menuName = "ScriptableObjects/PowerUp/GravityMagnet", order = 0)]
    public class GravityMagnet : PowerUp
    {
        [SerializeField][Range(.5f, 15f)][Tooltip("Ai will use this power up when the detect another player at this distance in front of them")]
        private float _aiUseDistanceBeforeFallingOff;
        
        [SerializeField][Range(.5f, 20f)][Tooltip("Ai will not use this Power up under this speed.")]
        private float _aiUseMinSpeed;

        [SerializeField] private LayerMask _groundLayerMask;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            var _userRb = _userTransform.GetComponent<Rigidbody>();
            _userRb.velocity = Vector3.zero;
            Debug.Log("Gravity Magnet Power up used.");
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            if (_userRigidbody.velocity.sqrMagnitude < _aiUseMinSpeed * _aiUseMinSpeed) return false;
            var dir = _userRigidbody.velocity.normalized;
            return Physics.CheckSphere(_userRigidbody.position + dir * _aiUseDistanceBeforeFallingOff, 1f,
                _groundLayerMask) == false;
        }
    }
}