using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RoundLeaderboard
{
    public class LeaderboardHeaderRoundCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private Image _activeCounterImage;

        public void SetCounterText(string _text)
        {
            _counterText.SetText(_text);
        }
    }
}