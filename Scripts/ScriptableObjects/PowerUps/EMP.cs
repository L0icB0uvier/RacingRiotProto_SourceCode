using UnityEngine;

namespace ScriptableObjects.PowerUps
{
    [CreateAssetMenu(fileName = "EMP", menuName = "ScriptableObjects/PowerUp/EMP", order = 0)]
    public class EMP : PowerUp
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _duration;
        
        [SerializeField] private LayerMask _affectLayerMask;
        
        public override void UsePowerUp(Transform _userTransform)
        {
            var collidersInRadius = Physics.OverlapSphere(_userTransform.position, _radius, _affectLayerMask);
            
            Debug.Log("EMP power up used.");

            foreach (var collider in collidersInRadius)
            {
                if (collider.transform == _userTransform) continue;
                var _strokeComponent = collider.GetComponent<PlayerStroke>();

                if (_strokeComponent == null)
                {
                    Debug.LogWarning("Tried to use emp on a game object that doesn't have a PlayerStroke component.");
                    continue;
                }
                
                _strokeComponent.StartCooldown(_duration);
            }
        }

        public override bool AreUseConditionMet(Rigidbody _userRigidbody)
        {
            return Physics.CheckSphere(_userRigidbody.position, _radius, _affectLayerMask);
        }
    }
}