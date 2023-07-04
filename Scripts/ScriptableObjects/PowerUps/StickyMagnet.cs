using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "StickyMagnet", menuName = "ScriptableObjects/PowerUp/StickyMagnet", order = 0)]
    public class StickyMagnet : PowerUp
    {
        [SerializeField][Range(1f, 20f)]
        private float _pullForce;

        [SerializeField][Range(1f, 20f)]
        private float _effectRadius;
        
        [SerializeField] 
        private LayerMask _affectLayerMask;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            Vector3 userPosition = _userTransform.position;
            var colliders = Physics.OverlapSphere(userPosition, _effectRadius, _affectLayerMask);
            
            foreach (var collider in colliders)
            {
                if(collider.transform == _userTransform) continue;
                
                var directionToUser = (userPosition - collider.transform.position).normalized;
                collider.GetComponent<Rigidbody>().AddForce(directionToUser * _pullForce, ForceMode.Impulse);
            }
            
            Debug.Log("Sticky magnet power up used.");
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            return Physics.CheckSphere(_userRigidbody.position, _effectRadius, _affectLayerMask);
        }
    }
}