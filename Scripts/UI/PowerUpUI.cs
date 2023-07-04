using System;
using Manager;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PowerUpUI : MonoBehaviour
    {
        [SerializeField] 
        private Button _button;
        
        [SerializeField] 
        private TMP_Text _powerUpNameText;

        [SerializeField] 
        private Image _chargingImage;
        
        private PowerUpController _powerUpController;

        private void Start()
        {
            PlayerManager.Instance.OnPlayersSpawned += OnPlayersSpawned;
            GameplayManager.Instance.OnRoundStart += Show;
            RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished;
            Hide();
        }

        private void Update()
        {
            if (_button.interactable == true) return;
            
            _chargingImage.fillAmount = _powerUpController.ChargeRatio;
        }

        private void OnPlayerTurnFinished(GameObject _player)
        {
            if (_player != PlayerManager.Instance.OwnerPlayer) return;
            Hide();
        }

        private void OnPlayersSpawned()
        {
            _powerUpController = PlayerManager.Instance.OwnerPlayer.GetComponent<PowerUpController>();
            _powerUpController.OnPowerUpCharged += EnableButton;
            _powerUpController.OnPowerUpUsed += DisableButton;
        
            _powerUpNameText.SetText(_powerUpController.PowerUpAssigned.PowerUpName);
            _button.onClick.AddListener(OnTriggerPowerUpButtonClicked);
            DisableButton();
        }

        private void OnTriggerPowerUpButtonClicked()
        {
            _powerUpController.TriggerPowerUp();
        }

        private void DisableButton()
        {
            _button.interactable = false;
        }

        private void EnableButton()
        {
            _button.interactable = true;
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
