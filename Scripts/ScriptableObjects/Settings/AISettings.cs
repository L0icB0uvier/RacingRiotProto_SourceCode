using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "AISettings", menuName = "ScriptableObjects/Settings/AISettings", order = 0)]
    public class AISettings : ScriptableObject
    {
        [SerializeField][Range(0,1)]
        private float _strokeErrorMargin;

        [SerializeField] 
        private Vector2 _strokeReactionTime;
        

        public float StrokeErrorMargin => _strokeErrorMargin;

        public Vector2 StrokeReactionTime => _strokeReactionTime;
    }
}