using System;
using UnityEditor;
using UnityEngine;

namespace Gameplay
{
    [CustomEditor(typeof(Checkpoint))]
    public class CheckpointCustomEditor : Editor
    {
        private SerializedProperty _requirePreviousCheckpoint;
        private SerializedProperty _previousCheckpoint;

        private void OnEnable()
        {
            _requirePreviousCheckpoint = serializedObject.FindProperty("_requirePreviousCheckpoint");
            _previousCheckpoint = serializedObject.FindProperty("_previousCheckpoint");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_requirePreviousCheckpoint);
            
            if (_requirePreviousCheckpoint.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_previousCheckpoint);
                EditorGUI.indentLevel--;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}