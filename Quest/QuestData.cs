using UnityEngine;

public enum QuestType
{
    Main,   // 메인 퀘스트 -> 스토리 진행과 관련있는 주된 퀘스트
    Sub     // 서브 퀘스트 -> 스토리와 상관없는 퀘스트
}

public enum QuestState
{
    ACCEPTABLE = 0,
    ONGOING = 1,
    COMPLETABLE = 2
}

[CreateAssetMenu(menuName = "Scriptable Object/QuestData", fileName = "Quest_Data")]
public class QuestData : ScriptableObject
{
    [SerializeField]
    private int _id;
    public int ID
    {
        get { return _id; }
    }

    [SerializeField]
    private QuestType _type;
    public QuestType Type
    {
        get { return _type; }
    }

    [SerializeField]
    private string _title;
    public string Title
    {
        get { return _title; }
    }

    [SerializeField]
    [TextArea]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    // 달성 조건

    public bool IsMainQuest
    {
        get { return _type == QuestType.Main; }
    }
}