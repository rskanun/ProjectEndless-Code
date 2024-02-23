using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerData playerData = (PlayerData)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("플레이어 데이터 초기화"))
        {
            playerData.Initialization();
        }
    }
}