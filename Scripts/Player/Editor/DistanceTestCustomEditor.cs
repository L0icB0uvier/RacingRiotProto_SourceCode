using System;
using UnityEditor;
using UnityEngine;

namespace Player
{
    [CustomEditor(typeof(DistanceTestController))]
    public class DistanceTestCustomEditor : Editor
    {
        private DistanceTestController _distanceTestController;

        private void OnEnable()
        {
            _distanceTestController = (DistanceTestController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Test distance"))
            {
                _distanceTestController.DistanceTest();
            }
        }
    }
}