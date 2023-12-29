using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 스크립터블 오브젝트")]
    [SerializeField] private GameData gameData;

    private ScriptResource scriptResource;

    private void Start()
    {
        scriptResource = ScriptResource.Instance;

        StartGame();
    }

    public void StartGame()
    {
        LoadScript();
    }

    private void LoadScript()
    {
        int chapter = gameData.ChapterNum;
        int root = gameData.RootNum;
        int subChapter = gameData.SubChapterNum;

        scriptResource.LoadScript(chapter, root, subChapter);
    }
}