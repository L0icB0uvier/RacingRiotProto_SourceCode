using System;
using UnityEngine;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {
        private string _playerName;

        private void Awake()
        {
            _playerName = AINamesGenerator.Utils.GetRandomName();
        }

        public string PlayerName => _playerName;
    }
}