using TMPro;
using UnityEngine;

namespace UI.RoundLeaderboard
{
    public class LeaderboardRoundScore : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _roundScoreText;

        public void SetScore(int _score)
        {
            _roundScoreText.SetText(_score.ToString());
        }

        public void Show()
        {
            _roundScoreText.enabled = true;
        }

        public void Hide()
        {
            _roundScoreText.enabled = false;
        }
    }
}