using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using ScriptableObjects.Settings;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace AI
{
    public class PlayerAIController : MonoBehaviour
    {
        [SerializeField] 
        private AISettings _aiSettings;
        
        [SerializeField] 
        private AnimationCurve _distanceCurve;

        [SerializeField] 
        private int _checkDirectionResolution;
        
        [SerializeField] 
        private LayerMask _obstacleLayersMask;

        [SerializeField] 
        private LayerMask _finishLineLayerMask;
        
        [SerializeField] 
        private float _updateRate = .2f;
        
        [SerializeField] 
        private float _sphereCastRadius = .6f;

        [SerializeField] 
        private int _searchResolution;
        
        [SerializeField]
        private float _searchDistanceAhead = 60;

        [SerializeField] 
        private float _groundTestingRayDistance;

        [SerializeField] 
        private LayerMask _groundLayerMask;

        [SerializeField] 
        private bool _showDebugInfo;
        
        private PlayerStroke _playerStroke;
        
        [SerializeField]
        private SplineContainer _trackSpline;
        
        private bool _behaviorActive;
        private bool _canStroke;
        private float _timeBeforeUpdate;

        private bool _inStartingGrid;
        private Transform _startingGridTransform;

        private TrajectoryInfo _trajectoryInfo;


        private void Awake()
        {
            _playerStroke = GetComponent<PlayerStroke>();
        }

        private void Start()
        {
            GameplayManager.Instance.OnRoundStart += StartAIBehavior;
            GameplayManager.Instance.OnRoundOver += StopAIBehavior;
            RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished;
            _playerStroke.OnStrokeEnabled += () => StartCoroutine(DelayBeforeStrokeCoroutine());
            LayoutLoader.Instance.OnLayoutReady += GetSpline;
            _playerStroke.OnStrokeDisabled += () => _canStroke = false;
        }

        private void GetSpline()
        {
            _trackSpline = GameObject.FindWithTag("TrackSpline").GetComponent<SplineContainer>();
        }

        private void OnPlayerTurnFinished(GameObject _playerFinished)
        {
            if (_playerFinished == gameObject)
            {
                StopAIBehavior();
            }
        }

        private IEnumerator DelayBeforeStrokeCoroutine()
        {
            var randomReactionTime = Random.Range(_aiSettings.StrokeReactionTime.x, _aiSettings.StrokeReactionTime.y);
            yield return new WaitForSeconds(randomReactionTime);
            _canStroke = true;
        }

        private void StopAIBehavior()
        {
            _behaviorActive = false;
            StopAllCoroutines();
        }

        private void StartAIBehavior()
        {
            _behaviorActive = true;
            _timeBeforeUpdate = 0;
        }

        private void Update()
        {
            if (_behaviorActive == false) return;
            
            var playerTransform = transform;
            var forward = playerTransform.forward;

            if (_timeBeforeUpdate <= 0)
            {
                if (_inStartingGrid)
                {
                    Vector3 direction = _startingGridTransform.forward;

                    var randomDeviation = Random.Range(-10, 10);
                    direction = Quaternion.AngleAxis(randomDeviation, Vector3.up) * direction;
                    _trajectoryInfo = new TrajectoryInfo(direction, transform.position + direction * GetMaxDistance(),
                        GetMaxDistance());
                    _timeBeforeUpdate = _updateRate;
                }

                else
                {
                    _trajectoryInfo = FindBestDirection();
                    _timeBeforeUpdate = _updateRate;
                }
            }
            
            _playerStroke.RotatePlayer(_trajectoryInfo.startDirection);
            
            var angleToStrokeDirection = Vector3.Angle(forward,
                _trajectoryInfo.startDirection);
            
            if (angleToStrokeDirection < 5 && _playerStroke.StrokeEnabled && _canStroke)
            {
                Stroke(_trajectoryInfo);
            }
            
            _timeBeforeUpdate -= Time.deltaTime;
        }

        [ContextMenu("Check Environment")]
        private TrajectoryInfo FindBestDirection()
        {
            var playerPosition = transform.position;
            SplineUtility.GetNearestPoint(_trackSpline.Spline, playerPosition, out float3 closestPoint, out float t);
            var targetPoint = (Vector3)_trackSpline.Spline.GetPointAtLinearDistance(t, _searchDistanceAhead, out var resultT);
            
            if (_showDebugInfo)
            {
                DebugExtension.DebugWireSphere(targetPoint, Color.yellow, 2f, _updateRate);
            }
            
            Vector3 targetDir = (targetPoint - playerPosition).normalized;

            List<TrajectoryInfo> trajectoriesInfo = new List<TrajectoryInfo>();

            int steps = Mathf.RoundToInt(360 / _checkDirectionResolution);

            for (int i = 0; i < _checkDirectionResolution; i++)
            {
                var rayDirection = Quaternion.AngleAxis(steps * i, Vector3.up) * targetDir;

                var rayTrajectoryEndLocationInfo = GetDirectionTrajectoryEstimate(rayDirection);
                trajectoriesInfo.Add(rayTrajectoryEndLocationInfo);
            }
            
            return FindBestTrajectory(trajectoriesInfo);
        }

        private TrajectoryInfo FindBestTrajectory(List<TrajectoryInfo> _trajectories)
        {
            float tMax = 0;
            TrajectoryInfo bestTrajectory = new TrajectoryInfo();
            
            foreach (var trajectoryInfo in _trajectories)
            {
                SplineUtility.GetNearestPoint(_trackSpline.Spline, trajectoryInfo.trajectoryDestination,
                    out float3 closestPoint, out float t);

                if (_showDebugInfo)
                {
                    Debug.DrawLine(trajectoryInfo.trajectoryDestination, closestPoint, Color.magenta, _updateRate);
                    DebugExtension.DebugWireSphere(closestPoint.xyz, Color.magenta, 2f, _updateRate);
                }

                if (t < tMax) continue;
                
                tMax = t;
                bestTrajectory = trajectoryInfo;
            }

            if (_showDebugInfo)
            {
                DebugExtension.DebugWireSphere(bestTrajectory.trajectoryDestination, Color.yellow, 2f, _updateRate);
            }
            
            return bestTrajectory;
        }
        
        private TrajectoryInfo GetDirectionTrajectoryEstimate(Vector3 _direction)
        {
            var aiTransform = transform;
            var rayStartPos = aiTransform.position;
            var rayStartDirection = _direction;
            float maxDistance = GetMaxDistance();
            float remainingDistance = maxDistance;
            float trajectoryDistance = 0;

            TrajectoryInfo trajectoryInfo = new TrajectoryInfo();
            trajectoryInfo.startDirection = _direction;

            List<Vector3> positions = new List<Vector3>();
            positions.Add(rayStartPos);

            while (remainingDistance > 0)
            {
                var rayDestination = rayStartPos + rayStartDirection * _searchResolution;
                
                //Check for ground at ray destination and return if no ground found
                if (Physics.Raycast(rayDestination + Vector3.up * 2, Vector3.down, out RaycastHit hit,
                        _groundTestingRayDistance,
                        _groundLayerMask) == false)
                {
                    if (_showDebugInfo)
                    {
                        DebugExtension.DebugWireSphere(rayDestination, Color.red, 1f, _updateRate);
                    }
                    
                    trajectoryInfo.trajectoryDestination = positions.Last();
                    trajectoryInfo.trajectoryLength = trajectoryDistance;
                    return trajectoryInfo;
                }

                //Correct Y position of the ray destination to account for non flat terrains
                rayDestination.y = hit.point.y + .5f;

                //Get the direction and distance to the corrected ray destination for SphereCast
                var vectorToNextPos = rayDestination - rayStartPos;
                rayStartDirection = vectorToNextPos.normalized;
                var distanceToNextPoint = Mathf.Clamp(vectorToNextPos.magnitude, 0, remainingDistance);
                
                //Check for Finish line and return early if hitting it
                if (Physics.Linecast(rayStartPos, rayDestination, out RaycastHit finishLineHit,
                        _finishLineLayerMask, QueryTriggerInteraction.Collide))
                {
                    
                    trajectoryInfo.trajectoryDestination = finishLineHit.point;
                    trajectoryInfo.trajectoryLength = trajectoryDistance;
                    return trajectoryInfo;
                }
                
                bool hitSomething = Physics.SphereCast(rayStartPos, _sphereCastRadius, rayStartDirection, out hit, distanceToNextPoint,
                    _obstacleLayersMask);
                
                if (hitSomething)
                {
                    var hitPos = rayStartPos + rayStartDirection * hit.distance;
                    if (_showDebugInfo)
                    {
                        Debug.DrawLine(rayStartPos, hitPos, Color.green, _updateRate);
                        DebugExtension.DebugWireSphere( hitPos, Color.blue, .5f, _updateRate);
                    }
                    
                    remainingDistance -= hit.distance;
                    trajectoryDistance += hit.distance;
                    remainingDistance *= hit.collider.material.bounciness;
                    rayStartPos = hitPos;
                    rayStartDirection = Vector3.Reflect(rayStartDirection, hit.normal);
                    positions.Add(hitPos);
                }
                else
                {
                    if (_showDebugInfo)
                    {
                        Debug.DrawLine(rayStartPos, rayDestination, Color.green, _updateRate);
                    }
                    
                    positions.Add(rayDestination);
                    rayStartPos = rayDestination;
                    remainingDistance -= distanceToNextPoint;
                    trajectoryDistance += distanceToNextPoint;
                }
            }
            
            trajectoryInfo.trajectoryDestination = positions.Last();
            trajectoryInfo.trajectoryLength = trajectoryDistance;
            return trajectoryInfo;
        }

        private void Stroke(TrajectoryInfo _trajectoryInfo)
        {
            float strokeStrength;
            
            if (_trajectoryInfo.trajectoryLength >= GetMaxDistance())
            {
                strokeStrength = 1;
            }

            else
            {
                strokeStrength = Mathf.Clamp01(GetStrokeStrengthToReachDistance(_trajectoryInfo.trajectoryLength));
            }
            
            _playerStroke.Stroke(strokeStrength);
        }

        private float GetMaxDistance()
        {
            return _distanceCurve.keys[^1].time;
        }

        private float GetStrokeStrengthToReachDistance(float _distance)
        {
            var errorMargin = Random.Range(0, _aiSettings.StrokeErrorMargin);
            int sign = Random.Range(0, 2);
            errorMargin = sign == 0 ? errorMargin : -errorMargin;
            return _distanceCurve.Evaluate(_distance) + errorMargin;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StartingGrid") == false) return;
            
            _inStartingGrid = true;
            _startingGridTransform = other.transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("StartingGrid") == false) return;

            _inStartingGrid = false;
            _startingGridTransform = null;
        }
        
        private struct TrajectoryInfo
        {
            public Vector3 startDirection;
            public Vector3 trajectoryDestination;
            public float trajectoryLength;

            public TrajectoryInfo(Vector3 _startDirection, Vector3 _trajectoryDestination, float _trajectoryLength)
            {
                startDirection = _startDirection;
                trajectoryDestination = _trajectoryDestination;
                trajectoryLength = _trajectoryLength;
            }
        }
    }
}