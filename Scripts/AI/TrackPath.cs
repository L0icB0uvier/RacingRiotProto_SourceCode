using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace AI
{
    public class TrackPath : MonoBehaviour
    {
        public static TrackPath Instance;

        [SerializeField] 
        private Transform _finishLine;
        
        [SerializeField] 
        private Transform[] _pathPoints;
        
        private LinkedList<Transform> _path;

        private void Awake()
        {
            Instance = this;
            _path = new LinkedList<Transform>();
            foreach (var pathCheckpoint in _pathPoints)
            {
                _path.AddLast(pathCheckpoint);
            }
        }

        public Transform GetFirstCheckpoint => _path.First.Value;

        public Transform GetNextCheckpoint(Transform _lastCheckpoint)
        {
            var currentNode = _path.Find(_lastCheckpoint);
            if (currentNode == null)
            {
                Debug.LogError($"Could not find {_lastCheckpoint.gameObject.name} in the path.");
                return null;
            }

            var nextNode = currentNode.Next;
            return nextNode == null ? _finishLine : nextNode.Value;
        }

        [ContextMenu("Get PathCheckpoints")]
        public void GetPathCheckpoint()
        {
            _pathPoints = new Transform[transform.childCount];
            
            for (int i = 0; i <  transform.childCount; i++)
            {
                _pathPoints[i] = transform.GetChild(i);
            }
        }

        public Transform FinishLine => _finishLine;
    }
}