using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class RespawnManager : MonoBehaviour
    {
        private Transform[] _respawnPoints;
        
        private void Start()
        {
            LayoutLoader.Instance.OnLayoutReady += RespawnPlayers;
        }
        
        private List<Transform> GetRespawnPoints()
        {
            return GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform).ToList();
        }

        private void RespawnPlayers()
        {
            var availableRespawnPoints = GetRespawnPoints();

            foreach (var player in PlayerManager.Instance.Players)
            {
                var randomIndex = Random.Range(0, availableRespawnPoints.Count);
                Transform spawnPoint = availableRespawnPoints[randomIndex];
                player.GetComponent<PlayerRespawn>().Respawn(spawnPoint.position, spawnPoint.rotation);
                availableRespawnPoints.RemoveAt(randomIndex);
            }
            
            GameplayManager.Instance.ChangeState(GameplayManager.EGameStates.RoundStartCountdown);
        }
    }
}