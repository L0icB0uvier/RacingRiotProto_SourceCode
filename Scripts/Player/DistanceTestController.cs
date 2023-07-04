using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class DistanceTestController : MonoBehaviour
    {
        [SerializeField][Range(0,1)]
        float _inputStrength = 1;

        [SerializeField] private AnimationCurve _curve;
        
        private PlayerStroke _playerStroke;
        private Rigidbody _rb;
        
        private Vector3 _distanceTestStartPos;

        private List<Vector3> _collisionPoints = new List<Vector3>();

        private void Awake()
        {
            _playerStroke = GetComponent<PlayerStroke>();
            _rb = GetComponent<Rigidbody>();
        }

        public void DistanceTest()
        {
            _distanceTestStartPos = transform.position;
            _playerStroke.Stroke(_inputStrength);
            StartCoroutine(CheckPlayerStopped());
        }

        private IEnumerator CheckPlayerStopped()
        {
            _collisionPoints.Clear();
            yield return new WaitForSeconds(.5f);
            while (_rb.velocity.sqrMagnitude > .5f)
            {
                yield return null;
            }
            
            Debug.Log($"Distance travelled: {GetDistance()}");
        }

        private float GetDistance()
        {
            if (_collisionPoints.Count == 0)
            {
                return Vector3.Distance(_distanceTestStartPos, transform.position);
            }
            
            float distance = 0;
            for (int i = 0; i < _collisionPoints.Count; i++)
            {
                if (i == 0)
                {
                    distance = Vector3.Distance(_distanceTestStartPos, _collisionPoints[0]);
                }
                else
                {
                    distance += Vector3.Distance(_collisionPoints[i - 1], _collisionPoints[i]);
                }
            }

            distance += Vector3.Distance(_collisionPoints[^1], transform.position);
            
            return distance;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall") == false) return;
            _collisionPoints.Add(transform.position);
        }
    }
}
