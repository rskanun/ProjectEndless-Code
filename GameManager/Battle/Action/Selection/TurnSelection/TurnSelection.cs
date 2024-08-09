using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TurnSelection : MonoBehaviour, ISelection, IMoveHandler
{
    [Header("참조 스크립트")]
    [SerializeField] private Timeline timeline;
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
        timeline.isMovable = true;

        // 타임라인 삽입 아이콘 활성화
        timeline.SetActiveInsert(true);

        // 배치 가능한 최소 위치 설정
        SetMinIndex(action);
    }

    public void CloseSelection()
    {
        timeline.isMovable = false;

        // 타임라인 삽입 아이콘 비활성화
        timeline.SetActiveInsert(false);
    }

    public void UndoSelection()
    {
        CloseSelection();

        // 타임라인 원위치
        timeline.MoveStart();
    }

    public void ReopenSelection()
    {
        // 마지막 선택창이므로 재오픈 X
    }

    private void SetMinIndex(BattleAction action)
    {
        minIndex = sequence.GetMinIndex(action);

        // 삽입 아이콘 중앙에 위치시키기
        timeline.MoveIndex(minIndex);
    }

    /***************************************************************
    * [ 타임라인 선택 ]
    * 
    * 해당 액션을 어느 타임라인에 넣을 지 선택 처리
    ***************************************************************/

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            MoveNext();
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            MovePrev();
        }
    }

    public void MoveNext()
    {
        timeline.MoveNext();
    }

    public void MovePrev()
    {
        if (timeline.CenterIndex > minIndex)
        {
            timeline.MovePrev();
        }
    }

    public void InsertAction()
    {
        // 배치될 턴의 이전 행동의 턴 수 가져오기
        // (해당 턴 = 배치될 행동의 턴)
        BattleAction prevAction = sequence.GetTurnAction(timeline.CenterIndex);

        actionManager.SelectTurn(prevAction.remainTurn, timeline.CenterIndex);
    }
}