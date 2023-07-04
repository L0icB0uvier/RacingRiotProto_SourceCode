using Cinemachine;
using Manager;
using UnityEngine;

namespace Player
{
    public class PlayerCameraTargetSetter : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineVirtualCam;

        private void Awake()
        {
            _cinemachineVirtualCam = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            PlayerManager.Instance.OnPlayersSpawned += SetCameraTarget;
        }

        private void SetCameraTarget()
        {
            var playerTransform = PlayerManager.Instance.OwnerPlayer.transform;
            _cinemachineVirtualCam.Follow = playerTransform;
            _cinemachineVirtualCam.LookAt = playerTransform;
        }
    }
}