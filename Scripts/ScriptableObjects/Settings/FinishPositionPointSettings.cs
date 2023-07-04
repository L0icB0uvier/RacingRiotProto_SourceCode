using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "FinishPositionPoints", menuName = "ScriptableObjects/Settings/FinishPositionPoints", order = 0)]
    public class FinishPositionPointSettings : ScriptableObject
    {
        [SerializeField] 
        private IntIntDictionary _finishPositionPoints = new IntIntDictionary();

        public int GetPointsForPosition(int _position)
        {
            return _finishPositionPoints[_position];
        }
    }
}