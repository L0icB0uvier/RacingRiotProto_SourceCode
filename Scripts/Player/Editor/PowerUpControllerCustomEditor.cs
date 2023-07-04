using UnityEditor;

namespace Player
{
    [CustomEditor(typeof(PowerUpController))]
    public class PowerUpControllerCustomEditor : Editor
    {
        private SerializedProperty _pickRandomPowerUpAtStart;
        private SerializedProperty _powerUpSO;
        private SerializedProperty _availablePowerUp;
        private SerializedProperty _chargeDistance;

        private void OnEnable()
        {
            _pickRandomPowerUpAtStart = serializedObject.FindProperty("_pickRandomPowerUpAtStart");
            _powerUpSO = serializedObject.FindProperty("_powerUpSO");
            _availablePowerUp = serializedObject.FindProperty("_availablePowerUp");
            _chargeDistance = serializedObject.FindProperty("_chargeDistance");
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_pickRandomPowerUpAtStart);
            
            if (_pickRandomPowerUpAtStart.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_availablePowerUp);
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_powerUpSO);
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.PropertyField(_chargeDistance);
            serializedObject.ApplyModifiedProperties();
        }
    }
}