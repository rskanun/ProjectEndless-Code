using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TeleportEvent : IDialogueEvent
{
    public string teleportSceneName;
    public Vector2 teleportPos;
    public void Execute()
    {
        PlayerManager.CurrentPlayer.Teleport(teleportPos);

        // 만약 씬이 현재와 다른 씬인 경우
        if (GameData.Instance.MapScene != teleportSceneName)
        {
            // 해당 씬 로드
            SceneLoadManager.LoadFieldScene(
                teleportSceneName,
                UnloadSceneOptions.None,
                SceneFadeEffect.BlurFadeOut,
                SceneFadeEffect.BlurFadeIn,
                LoadingScreen.Loading
            );
        }
    }
}