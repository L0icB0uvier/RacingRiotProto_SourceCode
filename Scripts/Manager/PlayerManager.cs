using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        
        [SerializeField] 
        private AssetReferenceT<GameObject> _playerPrefabAssetRef;
        
        [SerializeField] 
        private AssetReferenceT<GameObject> _aiPrefabAssetRef;
        
        [SerializeField] 
        private Transform _spawnPosition;
        
        [SerializeField] 
        private int _playersAmount;

        [SerializeField][Tooltip("Used to spawn only AI players for debug purposes.")]
        private bool _spawnOnlyAI;
        
        public Action OnPlayersSpawned;
        
        private AsyncOperationHandle<GameObject> _playerPrefabLoadHandle;
        private AsyncOperationHandle<GameObject> _aiPrefabLoadHandle;
        
        private GameObject _ownerPlayer;
        private GameObject[] _players;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SpawnPlayers();
        }

        private void OnDestroy()
        {
            Addressables.Release(_playerPrefabLoadHandle);
            Addressables.Release(_aiPrefabLoadHandle);
        }
        
        private void SpawnPlayers()
        {
            StartCoroutine(SpawnPlayersCoroutine());
        }

        private IEnumerator SpawnPlayersCoroutine()
        {
            _playerPrefabLoadHandle = _playerPrefabAssetRef.LoadAssetAsync<GameObject>();
            _aiPrefabLoadHandle = _aiPrefabAssetRef.LoadAssetAsync<GameObject>();

            yield return _playerPrefabLoadHandle.WaitForCompletion();
            yield return _aiPrefabLoadHandle.WaitForCompletion();
            
            _players = new GameObject[_playersAmount];
            for (int i = 0; i < _playersAmount; i++)
            {
                if (i == 0)
                {
                    _players[i] = Instantiate(_spawnOnlyAI? _aiPrefabLoadHandle.Result : _playerPrefabLoadHandle.Result, _spawnPosition.position, _spawnPosition.rotation);
                    _ownerPlayer = _players[i];
                }

                else
                {
                    _players[i] = Instantiate( _aiPrefabLoadHandle.Result, _spawnPosition.position + Vector3.right * 1.2f * i, _spawnPosition.rotation);
                }
            }
            
            Debug.Log("Players spawned.");
            OnPlayersSpawned?.Invoke();
            GameplayManager.Instance.ChangeState(GameplayManager.EGameStates.PrepareRound);
        }
        
        public GameObject[] Players => _players;
        public GameObject OwnerPlayer => _ownerPlayer;
    }
}