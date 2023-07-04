using CustomUtilities;
using UnityEngine;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField][Range(0, 180)][Tooltip("The maximum angle to rotate from the current position.Should be 180 degree for allowing a complete rotation.")]
        private float _maxAngle;

        [SerializeField] 
        private bool _invertX;

        [SerializeField] 
        private bool _lockRotationToMaxAngle;

        [SerializeField] 
        private float _rotateSpeed;
        
        private Joystick _joystick;

        private PlayerStroke _playerStroke;

        private bool _holdingJoystick;
        
        private bool _inputEnabled;
        private bool _joystickHoldRegistered;
        private Vector3 _baseRotation;
        
        private void Awake()
        {
            _joystick = FindObjectOfType<Joystick>();
            _playerStroke = GetComponent<PlayerStroke>();
        }

        private void Start()
        {
            _joystick.OnHoldJoystick += OnHoldJoystick;
            _joystick.OnReleaseJoystick += OnReleaseJoystick;
        }
        
        private void OnReleaseJoystick()
        {
            _playerStroke.Stroke(-_joystick.Vertical);
        }

        private void OnHoldJoystick()
        {
            _baseRotation = transform.forward;
        }

        private void Update()
        {
            if (_joystick.HoldingJoystick == false) return;

            ProcessHorizontalInput();
        }

        private void ProcessHorizontalInput()
        {
            if(_lockRotationToMaxAngle == false && _joystick.Horizontal >= 1)
            {
                var rotation = (_invertX ? -_rotateSpeed : _rotateSpeed) * Time.deltaTime;
                _baseRotation = Quaternion.AngleAxis(rotation, Vector3.up) * _baseRotation;
            }
            
            else if (_lockRotationToMaxAngle == false && _joystick.Horizontal <= -1)
            {
                var rotation = (_invertX ? _rotateSpeed : -_rotateSpeed) * Time.deltaTime;
                _baseRotation = Quaternion.AngleAxis(rotation, Vector3.up) * _baseRotation;
            }

            var rotationAngle = _joystick.Horizontal * (_invertX ? -_maxAngle : _maxAngle);
            var rot = Quaternion.AngleAxis(rotationAngle, Vector3.up) * _baseRotation;
            _playerStroke.RotatePlayer(rot);
        }
    }
}