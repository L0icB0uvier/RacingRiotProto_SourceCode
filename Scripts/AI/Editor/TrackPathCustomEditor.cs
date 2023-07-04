using UnityEditor;
using UnityEngine;

namespace AI
{
    [CustomEditor(typeof(TrackPath))]
    public class TrackPathCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Get Path Checkpoints"))
            {
                var trackPath = (TrackPath)target;
                trackPath.GetPathCheckpoint();
            }
        }
    }
}
