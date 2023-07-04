using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI.RoundLeaderboard
{
    public class LeaderboardHeader : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<GameObject> _roundCountPrefabAssetRef;

        [SerializeField] private Transform _roundCountLabelsParent;
        
        private AsyncOperationHandle<GameObject> _roundCountPrefabLoadHandle;

        private LinkedList<LeaderboardHeaderRoundCounter> _roundCounters =
            new LinkedList<LeaderboardHeaderRoundCounter>();

        private void Awake()
        {
            _roundCountPrefabLoadHandle = _roundCountPrefabAssetRef.LoadAssetAsync<GameObject>();
            _roundCountPrefabLoadHandle.Completed += RoundCountPrefabLoadHandleOnCompleted;
        }

        private void RoundCountPrefabLoadHandleOnCompleted(AsyncOperationHandle<GameObject> _obj)
        {
            for (int i = 1; i <= GameplayManager.Instance.NumberOfRound; i++)
            {
                var roundCounter = Instantiate(_obj.Result, _roundCountLabelsParent)
                    .GetComponent<LeaderboardHeaderRoundCounter>();
                roundCounter.SetCounterText(i.ToString());
                _roundCounters.AddLast(roundCounter);
            }
        }
    }
}