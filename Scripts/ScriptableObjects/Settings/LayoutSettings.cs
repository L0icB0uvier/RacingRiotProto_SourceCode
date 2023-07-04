using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "LayoutSetting", menuName = "ScriptableObjects/Settings/LayoutSettings", order = 0)]
    public class LayoutSettings : ScriptableObject
    {
        [SerializeField] private LayoutSO[] _layouts;

        public LayoutSO[] Layouts => _layouts;
    }
}