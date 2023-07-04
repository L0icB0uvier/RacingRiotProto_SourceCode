using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Player;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI.RoundLeaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] 
        private Transform _leaderboardEntriesParent;
        
        [SerializeField] 
        private AssetReferenceT<GameObject> _leaderboardEntryPrefabAssetRef;

        [SerializeField] 
        private FinishPositionPointSettings _finishPositionPointSettings;

        [SerializeField] 
        private float _displayTime;
        
        private AsyncOperationHandle<GameObject> _leaderboardEntryPrefabLoadHandle;

        private List<PlayerLeaderboardInfo> _playersLeaderboardInfo = new List<PlayerLeaderboardInfo>();

        private void Start()
        {
            PlayerManager.Instance.OnPlayersSpawned += Initialize;
            GameplayManager.Instance.OnRoundOver += RetrievePoints;
            GameplayManager.Instance.OnInitialize += Reset;
        }

        private void Reset()
        {
            foreach (var playerLeaderboardInfo in _playersLeaderboardInfo)
            {
                playerLeaderboardInfo.playerLeaderboardEntry.Reset();
            }
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void RetrievePoints()
        {
            var timeOrderedLeaderboardInfo = _playersLeaderboardInfo.OrderBy(x => x.playerRoundPerformance.Timer);
            
            //Add a way to assign position to players who didn't reach the finish line
            
            int positionIndex = 1;
            
            foreach (var playerLeaderboardInfo in timeOrderedLeaderboardInfo)
            {
                int score = _finishPositionPointSettings.GetPointsForPosition(positionIndex);
                playerLeaderboardInfo.playerLeaderboardEntry.UpdateRoundScoreEntry(GameplayManager.Instance.CurrentRound, score);
                positionIndex++;
            }
            
            ReorderEntries();
            Show();
            StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_displayTime);
            Hide();
            GameplayManager.Instance.RoundComplete();
        }

        private void OnDestroy()
        {
            Addressables.Release(_leaderboardEntryPrefabLoadHandle);
        }

        private void Initialize()
        {
            _leaderboardEntryPrefabLoadHandle = _leaderboardEntryPrefabAssetRef.LoadAssetAsync<GameObject>();
            _leaderboardEntryPrefabLoadHandle.Completed += GenerateLeaderboardEntries;
        }

        private void GenerateLeaderboardEntries(AsyncOperationHandle<GameObject> _obj)
        {
            foreach (var player in PlayerManager.Instance.Players)
            {
                var playerData = player.GetComponent<PlayerData>();
                var playerRoundPerformance = player.GetComponent<PlayerRoundPerformance>();
                var leaderboardEntry = Instantiate(_obj.Result, _leaderboardEntriesParent).GetComponent<LeaderboardEntry>();
                leaderboardEntry.Initialise(playerData.PlayerName, PlayerManager.Instance.OwnerPlayer == player);

                var playerLeaderboardInfo =
                    new PlayerLeaderboardInfo(playerData, playerRoundPerformance, leaderboardEntry);
                
                _playersLeaderboardInfo.Add(playerLeaderboardInfo);
            }
            
            Hide();
        }

        public void ReorderEntries()
        {
            _playersLeaderboardInfo = _playersLeaderboardInfo.OrderByDescending(x => x.playerLeaderboardEntry.TotalScore).ToList();

            for (int i = 0; i < _playersLeaderboardInfo.Count; i++)
            {
                _playersLeaderboardInfo[i].playerLeaderboardEntry.UpdateSiblingIndex(i + 1);
                _playersLeaderboardInfo[i].playerLeaderboardEntry.UpdateRank(i + 1);
            }
        }
    }

    public class PlayerLeaderboardInfo
    {
        public PlayerData playerData;
        public PlayerRoundPerformance playerRoundPerformance;
        public LeaderboardEntry playerLeaderboardEntry;

        public PlayerLeaderboardInfo(PlayerData _playerData, PlayerRoundPerformance _playerRoundPerformance,
            LeaderboardEntry _leaderboardEntry)
        {
            playerData = _playerData;
            playerRoundPerformance = _playerRoundPerformance;
            playerLeaderboardEntry = _leaderboardEntry;
        }
    }
}