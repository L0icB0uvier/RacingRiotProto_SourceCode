using System;
using Cinemachine;
using Manager;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        
        private bool _canFreelook;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _canFreelook = true;
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }

        public void EnableFreeLook()
        {
            _canFreelook = true;
        }

        public void DisableFreeLook()
        {
            _canFreelook = false;
        }
        public float GetAxisCustom(string _axisName)
        {
            if (_axisName == "Mouse X")
            {
                if (Input.GetMouseButton(0) && _canFreelook)
                {
                    return Input.GetAxis("Mouse X");
                }
                else
                {
                    return 0;
                }
            }
            else if (_axisName == "Mouse Y")
            {
                if (Input.GetMouseButton(0) && _canFreelook)
                {
                    return Input.GetAxis("Mouse Y");
                }
                else
                {
                    return 0;
                }
            }
            return Input.GetAxis(_axisName);
        }
    }
}
