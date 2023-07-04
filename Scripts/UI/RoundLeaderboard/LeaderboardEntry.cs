using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.RoundLeaderboard
{
    public class LeaderboardEntry : MonoBehaviour
    {
        [SerializeField] private Image _entryBackground;
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _playerTotalScoreText;

        [SerializeField] private Transform _roundScoresParent;
        [SerializeField] private AssetReferenceT<GameObject> _roundScorePrefabAssetRef;

        [SerializeField] private Color _playerBackgroundColor;
        [SerializeField] private Color _defaultBackgroundColor;
        
        private Dictionary<int, LeaderboardRoundScore> _roundScores = new Dictionary<int, LeaderboardRoundScore>();

        private AsyncOperationHandle<GameObject> _roundScorePrefabLoadHandle;

        private int _totalScore;

        private void Awake()
        {
            _roundScorePrefabLoadHandle = _roundScorePrefabAssetRef.LoadAssetAsync<GameObject>();
            _roundScorePrefabLoadHandle.Completed += RoundScorePrefabLoadHandleOnCompleted;
        }

        private void RoundScorePrefabLoadHandleOnCompleted(AsyncOperationHandle<GameObject> _obj)
        {
            for (int i = 1; i <= GameplayManager.Instance.NumberOfRound; i++)
            {
                var roundScore = Instantiate(_roundScorePrefabLoadHandle.Result, _roundScoresParent).GetComponent<LeaderboardRoundScore>();
                roundScore.Hide();
                _roundScores[i] = roundScore;
            }
        }

        private void OnDestroy()
        {
            Addressables.Release(_roundScorePrefabLoadHandle);
        }

        public void Reset()
        {
            _totalScore = 0;
            _playerTotalScoreText.SetText(_totalScore.ToString());
            foreach (var leaderboardRoundScore in _roundScores)
            {
                leaderboardRoundScore.Value.Hide();
            }
        }

        public void Initialise(string _playerName, bool _isPlayer)
        {
            _playerNameText.SetText(_playerName);
            _entryBackground.color = _isPlayer? _playerBackgroundColor : _defaultBackgroundColor;
            UpdateTotalScore(0);
        }

        public void UpdateRoundScoreEntry(int _roundNumber, int _roundScore)
        {
            var roundScore = _roundScores[_roundNumber];
            roundScore.SetScore(_roundScore);
            roundScore.Show();
            UpdateTotalScore(_totalScore + _roundScore);
        }

        private void UpdateTotalScore(int _newScore)
        {
            _totalScore = _newScore;
            _playerTotalScoreText.SetText(_totalScore.ToString());
        }

        public void UpdateSiblingIndex(int _newIndex)
        {
            transform.SetSiblingIndex(_newIndex);
        }

        public void UpdateRank(int _newRank)
        {
            _rankText.SetText(_newRank.ToString());
        }

        public int TotalScore => _totalScore;
    }
}