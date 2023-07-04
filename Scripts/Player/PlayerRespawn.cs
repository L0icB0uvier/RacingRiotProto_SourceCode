using Manager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

namespace Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        public UnityEvent OnRespawn;
        
        private Rigidbody _rigidbody;
        private Vector3 _positionSaved;
        private Quaternion _rotationSaved;

        private Spline _spline;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            LayoutLoader.Instance.OnLayoutReady += GetLayoutTrackSpline;
        }

        private void GetLayoutTrackSpline()
        {
            _spline = GameObject.FindWithTag("TrackSpline").GetComponent<SplineContainer>().Spline;
        }

        public void SaveCurrentPositionAndRotation()
        {
            SplineUtility.GetNearestPoint(_spline, transform.position, out float3 closestPoint, out float t);
            Vector3 splineTangent = _spline.EvaluateTangent(t);
            
            _positionSaved = closestPoint.xyz;
            _rotationSaved = Quaternion.LookRotation(splineTangent.normalized);
        }

        public void RespawnAtLastSavedPosition()
        {
            Respawn(_positionSaved, _rotationSaved);
        }
        
        public void Respawn(Vector3 _position, Quaternion _rotation)
        {
            
            transform.SetPositionAndRotation(_position, _rotation);
            _rigidbody.velocity = Vector3.zero;
            OnRespawn?.Invoke();
        }
    }
}