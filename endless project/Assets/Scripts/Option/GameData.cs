using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/GameData", fileName = "Game_Data")]
public class GameData : ScriptableObject
{
    /************************************************************
    * [챕터 데이터]
    * 
    * 현재 플레이어가 진행 중인 챕터(1~9), 분기 번호, 챕터 내
    * 구간을 나눈 서브 챕터 번호 데이터
    ************************************************************/

    [SerializeField]
    private int _chapterNum = 9;
    public int ChapterNum
    {
        get { return _chapterNum; }
        set
        {
            if (value < 0) _chapterNum = 0;
            else _chapterNum = value;
        }
    }

    [SerializeField]
    private int _rootNum;
    public int RootNum
    {
        get { return _rootNum; }
        set
        {
            if (value < 0) _rootNum = 0;
            else _rootNum = value;
        }
    }

    [SerializeField]
    private int _subChapterNum;
    public int SubChapterNum
    {
        get { return _subChapterNum; }
        set
        {
            if (value < 0) _subChapterNum = 0;
            else _subChapterNum = value;
        }
    }

    /************************************************************
    * [퀘스트 데이터]
    * 
    * 현재 플레이어가 진행 중인 퀘스트 관련 데이터
    ************************************************************/

    [SerializeField]
    private QuestData _questData;
    public QuestData MainQuest
    {
        get { return _questData; }
        set { _questData = value; }
    }

    /************************************************************
    * [맵 데이터]
    * 
    * 현재 플레이어가 있는 지형 관련 데이터
    ************************************************************/

    [SerializeField]
    private MapData _mapData;
    public MapData MapData
    {
        get { return _mapData; }
        set
        {
            if (_mapData.ID.Equals(value.ID) == false)
            {
                // 현재 있는 맵과 다른 맵일 경우 씬 이동
                MapManager.LoadMap(_mapData);
            }

            _mapData = value;
        }
    }
}