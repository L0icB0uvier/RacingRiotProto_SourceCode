using System;
using System.Collections.Generic;
using Gameplay;
using Manager;
using UnityEngine;

namespace Player
{
    public class TrackProgress : MonoBehaviour
    {
        private HashSet<Checkpoint> _checkpointsReached = new HashSet<Checkpoint>();

        private void Start()
        {
            GameplayManager.Instance.OnPrepareRound += ClearCheckpoints;
        }

        private void ClearCheckpoints()
        {
            _checkpointsReached.Clear();
        }

        public void RegisterCheckpoint(Checkpoint _checkpoint)
        {
            if (_checkpoint.RequirePreviousCheckpoint && _checkpointsReached.Contains(_checkpoint.PreviousCheckpoint) == false)
            {
                Debug.Log($"{gameObject.name} can't register checkpoint because it didn't register the previous checkpoints.");
                return;
            }
            
            _checkpointsReached.Add(_checkpoint);
            Debug.Log($"{gameObject.name} registered {_checkpoint.name}");
        }

        public int CheckpointsCount => _checkpointsReached.Count;
    }
}