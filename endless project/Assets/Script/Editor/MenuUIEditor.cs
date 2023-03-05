using UnityEditor;
using UnityEngine;

namespace Assets.Script.Interface.Menu
{
    [CustomEditor(typeof(PhoneOptionSetting))]
    [CanEditMultipleObjects]
    public class MenuUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PhoneOptionSetting setting = (PhoneOptionSetting)target;

            EditorGUILayout.BeginHorizontal();
            setting.Meridiem = (Meridiem)EditorGUILayout.EnumPopup("Time", setting.Meridiem);
            setting.Hour = EditorGUILayout.IntField(setting.Hour);
            setting.Minute = EditorGUILayout.IntField(setting.Minute);
            EditorGUILayout.EndHorizontal();
        }
    }
}