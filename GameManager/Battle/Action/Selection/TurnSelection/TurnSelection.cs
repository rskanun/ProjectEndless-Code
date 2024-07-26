using UnityEngine;

public class TurnSelection : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private Timeline timeline;
    [SerializeField] private TurnSelectionController controller;
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
        // 타임라인 삽입 아이콘 활성화
        timeline.SetActiveInsert(true);

        // 배치 가능한 최소 위치 설정
        SetMinIndex(action);

        // 컨트롤러 활성화
        controller.ActiveController();
    }

    public void CloseSelection()
    {
        // 컨트롤러 비활성화
        controller.DeactiveController();

        // 타임라인 삽입 아이콘 비활성화
        timeline.SetActiveInsert(false);
    }

    public void UndoSelection()
    {
        CloseSelection();

        // 타임라인 원위치
        timeline.MoveStart();

        actionManager.UndoTurn();
    }

    private void SetMinIndex(BattleAction action)
    {
        minIndex = sequence.GetMinIndex(action);

        // 삽입 아이콘 중앙에 위치시키기
        timeline.MoveIndex(minIndex);
    }

    public void MoveNext()
    {
        timeline.MoveNext();
    }

    public void MovePrev()
    {
        timeline.MovePrev();
        if (timeline.CenterIndex > minIndex)
        {
        }
    }

    public void InsertAction()
    {
        // 배치될 턴의 이전 행동의 턴 수 가져오기
        // (해당 턴 = 배치될 행동의 턴)
        // BattleAction prevAction = sequence.GetTurnAction(timeline.IconIndex - 1);
    }
}