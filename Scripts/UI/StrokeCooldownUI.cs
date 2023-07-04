using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StrokeCooldownUI : MonoBehaviour
    {
        private PlayerStroke _playerStroke;
        private Image _fillImage;

        private void Awake()
        {
            _fillImage = GetComponent<Image>();
        }

        private void Start()
        {
            PlayerManager.Instance.OnPlayersSpawned += OnPlayersSpawned;
            Hide();
        }

        private void OnPlayersSpawned()
        {
            _playerStroke = PlayerManager.Instance.Players[0].GetComponent<PlayerStroke>();
            _playerStroke.OnCooldownStart += InitializeCooldown;
            _playerStroke.OnCooldownEnd += Hide;
        }

        private void InitializeCooldown()
        {
            _fillImage.fillAmount = 1;
            Show();
        }

        private void Update()
        {
            var fillAmount = _playerStroke.CurrentCooldownTime / _playerStroke.CooldownDuration;
            _fillImage.fillAmount = fillAmount;
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}