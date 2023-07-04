using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "AirPush", menuName = "ScriptableObjects/PowerUp/AirPush", order = 0)]
    public class AirPush : PowerUp
    {
        [SerializeField][Range(1f, 20f)]
        private float _pushForce;
        [SerializeField][Range(0f, 1f)]
        private float _upwardModifier = 0;
        
        [SerializeField][Range(1f, 20f)]
        private float _effectRadius;
        
        [SerializeField] 
        private LayerMask _affectLayerMask;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            var colliders = Physics.OverlapSphere(_userTransform.position, _effectRadius, _affectLayerMask);
            
            foreach (var collider in colliders)
            {
                if(collider.transform == _userTransform) continue;
                collider.GetComponent<Rigidbody>().AddExplosionForce(_pushForce, _userTransform.position, _effectRadius, _upwardModifier, ForceMode.Impulse);
            }
            
            Debug.Log("Air push power up used.");
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            return Physics.CheckSphere(_userRigidbody.position, _effectRadius, _affectLayerMask);
        }
    }
}