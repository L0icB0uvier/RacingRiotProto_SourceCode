using Gameplay;
using Player;
using UnityEngine;

namespace Manager
{
    public class FinishLine : MonoBehaviour
    {
        [SerializeField] 
        private GameObject[] _checkpoints;

        [ContextMenu("Get Checkpoints")]
        private void GetCheckpoints()
        {
            _checkpoints = GameObject.FindGameObjectsWithTag("PathCheckpoint");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (GameplayManager.Instance.CurrentState != GameplayManager.EGameStates.Playing) return;
                var playerTrackProgress = other.GetComponent<TrackProgress>();
                
                if (playerTrackProgress.CheckpointsCount != _checkpoints.Length)
                {
                    Debug.Log($"{other.gameObject.name} reached the finish line but didn't clear all checkpoints");
                    return;
                }
                
                RoundEndManager.Instance.PlayerTurnFinished(other.gameObject);
            }
        }
    }
}