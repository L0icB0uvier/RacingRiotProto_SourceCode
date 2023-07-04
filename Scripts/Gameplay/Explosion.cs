using UnityEngine;

namespace Gameplay
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private LayerMask _affectedLayers;
        [SerializeField] private float _explosionStrength;
        [SerializeField] private float _radius;
        [SerializeField] private float _upwardModifier;
        [SerializeField] private ForceMode _forceMode;
        
        
        [ContextMenu("Explode")]
        private void Explode()
        {
            var colliders = Physics.OverlapSphere(transform.position, _radius, _affectedLayers);

            foreach (var collider1 in colliders)
            {
                collider1.GetComponent<Rigidbody>().AddExplosionForce(_explosionStrength, transform.position, 0, _upwardModifier, _forceMode);
            }
        }
    }
}