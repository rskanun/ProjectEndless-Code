using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TeleportEvent : IDialogueEvent
{
#if UNITY_EDITOR
    // 실재 존재하는 씬을 설정하기 위한 장치
    [SerializeField, OnValueChanged(nameof(OnMapChanged))]
    private SceneAsset loadMapScene;
#endif
    private string loadMapSceneName;

    [SerializeField]
    private Vector2 teleportPos;
    public void Execute()
    {
        PlayerManager.CurrentPlayer.Teleport(teleportPos);

        // 만약 씬이 현재와 다른 씬인 경우
        if (GameData.Instance.MapScene != loadMapSceneName)
        {
            // 해당 씬 로드
            SceneLoadManager.LoadFieldScene(
                loadMapSceneName,
                UnloadSceneOptions.None,
                SceneFadeEffect.BlurFadeOut,
                SceneFadeEffect.BlurFadeIn,
                LoadingScreen.Loading
            );
        }
    }

    private void OnMapChanged()
    {
        loadMapSceneName = loadMapScene.name;
    }
}