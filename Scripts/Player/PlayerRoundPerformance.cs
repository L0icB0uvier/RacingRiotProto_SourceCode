using Manager;
using UnityEngine;

namespace Player
{
    public class PlayerRoundPerformance : MonoBehaviour
    {
        private PlayerStroke _playerStroke;
        
        private int _strokeCount;
        private float _timer;

        private void Awake()
        {
            _playerStroke = GetComponent<PlayerStroke>();
        }

        private void Start()
        {
            _playerStroke.OnStroke += IncrementStrokeCount;
            GameplayManager.Instance.OnRoundStart += Initialize;
            GameplayManager.Instance.OnRoundOver += OnPlayerTurnEnd;
            RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished;
            OnPlayerTurnEnd();
        }

        private void OnPlayerTurnFinished(GameObject _player)
        {
            if (_player == gameObject)
            {
                OnPlayerTurnEnd();
            }
        }

        private void OnPlayerTurnEnd()
        {
            enabled = false;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
        }

        private void Initialize()
        {
            _strokeCount = 0;
            _timer = 0f;
            enabled = true;
        }

        private void IncrementStrokeCount()
        {
            _strokeCount++;
        }

        public int StrokeCount => _strokeCount;

        public float Timer => _timer;
    }
}