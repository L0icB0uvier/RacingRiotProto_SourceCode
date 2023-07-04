using Player;
using UnityEngine;

namespace Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private bool _requirePreviousCheckpoint;

        [SerializeField]
        private Checkpoint _previousCheckpoint;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") == false) return;
            
            other.GetComponent<TrackProgress>().RegisterCheckpoint(this);
        }

        public bool RequirePreviousCheckpoint => _requirePreviousCheckpoint;

        public Checkpoint PreviousCheckpoint => _previousCheckpoint;
    }
}