using Manager;
using UnityEngine;

public class DisplayOnPlaying : MonoBehaviour
{
    private void Start()
    {
        GameplayManager.Instance.OnPrepareRound += Show;
        RoundEndManager.Instance.OnPlayerTurnFinished += OnPlayerTurnFinished; 
    }

    private void OnPlayerTurnFinished(GameObject _player)
    {
        if (_player != PlayerManager.Instance.OwnerPlayer) return;
        Hide();
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
