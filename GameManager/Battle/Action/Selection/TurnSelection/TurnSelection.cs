using UnityEngine;

public class TurnSelection : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TurnSelectionUI ui;
    [SerializeField] private TimelineUI timelineUI;
    [SerializeField] private ActionManager actionManager;

    // 참조 데이터
    private BattleSequence sequence;

    // 최소 배치 가능 Index
    private int minIndex;

    private void Awake()
    {
        sequence = BattleData.Instance.Sequence;
    }

    public void OpenSelection(BattleAction action)
    {
        // UI 설정
        timeline.SetActiveInsertMode(true);

        // 배치 가능한 최소 위치 설정
        SetMinIndex(action);
    }

    public void CloseSelection()
    {
        timeline.SetActiveInsertMode(false);
    }

    public void UndoSelection()
    {
    }

    private void SetMinIndex(BattleAction action)
    {
        minIndex = sequence.GetMinIndex(action);

        // 현재 턴보단 뒤에 배치되어야 함
        if (minIndex <= 0)
            minIndex = 1;
    }

    public void MoveNextTimeline()
    {
        timeline.MoveToNext();
    }

    public void MovePrevTimeline()
    {
        if (timeline.IconIndex > minIndex)
        {
            timeline.MoveToPrev();
        }
    }

    public void InsertAction()
    {
        // 배치될 턴의 이전 행동의 턴 수 가져오기
        // (해당 턴 = 배치될 행동의 턴)
        BattleAction prevAction = sequence.GetTurnAction(timeline.IconIndex - 1);
    }
}