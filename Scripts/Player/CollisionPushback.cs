using System;
using System.Collections;
using CustomUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class CollisionPushback : MonoBehaviour
    {
        [SerializeField] 
        private float _maxPushBackStrength = 15f;

        [SerializeField] 
        private float _minPushbackStrength = 1f;
        
        [SerializeField] 
        private float _pushbackResistance = 1f;
        
        [SerializeField] 
        private float _verticalStrength = .5f;
        
        [SerializeField] 
        private float _velocityCap = 20;

        public UnityEvent OnPushedBack;
        
        private float _pushbackStrengthModifier = 1;
        private float _pushbackResistanceModifier = 1;
        
        private Rigidbody _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled == false || collision.gameObject.CompareTag("Player") == false) return;

            var finalPushbackStrength = GetPushbackStrength(collision.transform) * _pushbackStrengthModifier;

            var otherCollisionPushback = collision.gameObject.GetComponent<CollisionPushback>();
            otherCollisionPushback.ApplyPushback(finalPushbackStrength, collision.GetContact(0).point);
        }

        private float GetPushbackStrength(Transform _otherTransform)
        {
            var velocityMagnitude = _rb.velocity.magnitude;
            
            //If the player is not moving, return minimal force
            if (velocityMagnitude == 0)
            {
                return _minPushbackStrength;
            }
            
            var vectorToOther = _otherTransform.position - transform.position;
            var vectorToOtherNormalize = vectorToOther.normalized;
            var dot = Vector3.Dot(_rb.velocity.normalized, vectorToOtherNormalize);
            var directionalFactor = Mathf.Clamp01(dot);
            
            //If the player's velocity is not directed toward the colliding player, return minimal force
            if (directionalFactor == 0)
            {
                return _minPushbackStrength;
            }
            
            var cappedVelocity = Mathf.Clamp(_rb.velocity.magnitude, 0, _velocityCap);
            var impactForce = cappedVelocity * directionalFactor;

            return MathCalculation.Remap(impactForce, 0, _velocityCap, _minPushbackStrength, _maxPushBackStrength);
        }

        public void ApplyPushback(float _force, Vector3 _position)
        {
            var resistanceValue = _pushbackResistance * _pushbackResistanceModifier;
            _force /= resistanceValue;
            
            _rb.AddExplosionForce(_force, _position, 0, _verticalStrength, ForceMode.Impulse);
            OnPushedBack?.Invoke();
        }

        public void ApplyPushbackStrengthModifierForDuration(float _modifier, float _duration)
        {
            _pushbackStrengthModifier = _modifier;
            StartCoroutine(RevertPushbackStrengthModifierAfterDuration(_duration));
        }

        private IEnumerator RevertPushbackStrengthModifierAfterDuration(float _duration)
        {
            yield return new WaitForSeconds(_duration);
            _pushbackStrengthModifier = 1;
        }
        
        public void ApplyPushbackResistanceModifierForDuration(float _modifier, float _duration)
        {
            _pushbackResistanceModifier = _modifier;
            StartCoroutine(RevertPushbackResistanceModifierAfterDuration(_duration));
        }

        private IEnumerator RevertPushbackResistanceModifierAfterDuration(float _duration)
        {
            yield return new WaitForSeconds(_duration);
            _pushbackResistanceModifier = 1;
        }
    }
}
