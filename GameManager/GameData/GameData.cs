using Endless.GameData;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private Chapter _chapter = new Chapter(9, 0, 0);
    public Chapter Chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }

    /************************************************************
    * [날짜 데이터]
    * 
    * 현재 게임 내 날짜와 게임의 $&%와 관련된 데이터
    ************************************************************/

    [SerializeField]
    private Date _date = new Date(11, 19);
    public Date Date
    {
        get { return _date; }
        set { _date = value; }
    }

    [SerializeField]
    private RemainTime _time;
    public RemainTime Time
    {
        get
        {
            if (_time == null || _time.IsNull)
            {
                int loadTime = PlayerPrefs.GetInt("remainTime");

                if (loadTime <= 0) _time = new RemainTime(30227);
                else _time = new RemainTime(loadTime);
            }

            return _time;
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
        set { _mapData = value; }
    }
    private Tilemap _fieldTilemap;
    public Tilemap FieldTilemap
    {
        get { return _fieldTilemap; }
        set { _fieldTilemap = value; }
    }
}