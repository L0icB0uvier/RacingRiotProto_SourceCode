using Player;
using UnityEngine;

namespace Manager
{
    public class PlayerOutDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerRespawn>().RespawnAtLastSavedPosition();
            }
        }
    }
}