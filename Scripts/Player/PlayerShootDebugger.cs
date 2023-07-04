using UnityEngine;

namespace Player
{
    public class PlayerShootDebugger : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _debugLine;
        [SerializeField] 
        private float _maxPullBackDistance;

        [SerializeField] 
        private Gradient _strokeDisabledGradient;
        [SerializeField] 
        private Gradient _strokeEnabledGradient;
        
        private PlayerStroke _playerStroke;
        private Joystick _joystick;
        
        private bool _showingDebugLine;

        private void Awake()
        {
            _joystick = FindObjectOfType<Joystick>();
            _playerStroke = GetComponent<PlayerStroke>();
        }
        
        private void Start()
        {
            _joystick.OnHoldJoystick += ShowDebugLine;
            _joystick.OnReleaseJoystick += HideDebugLine;
            
            _playerStroke.OnStrokeEnabled += SetStrokeEnabledStyle;
            _playerStroke.OnStrokeDisabled += SetStrokeDisabledStyle;
        }
        
        private void SetStrokeEnabledStyle()
        {
            _debugLine.colorGradient = _strokeEnabledGradient;
        }

        private void SetStrokeDisabledStyle()
        {
            _debugLine.colorGradient = _strokeDisabledGradient;
        }

        private void HideDebugLine()
        {
            enabled = false;
            _debugLine.enabled = false;
        }

        private void ShowDebugLine()
        {
            enabled = true;
            _debugLine.enabled = true;
        }

        private void Update()
        {
            var playerTransform = transform;
            var startPoint = playerTransform.position;
            var dragPoint = startPoint + playerTransform.forward * Mathf.Clamp(_joystick.Vertical, -1, 0)  * _maxPullBackDistance;
            var inverseDragPoint = startPoint + (startPoint - dragPoint);

            _debugLine.SetPositions(new Vector3[] { startPoint, inverseDragPoint });
        }
    }
}