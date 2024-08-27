using UnityEngine;
using UnityEngine.EventSystems;

public class TurnSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private Timeline timeline;
    [SerializeField] private TurnSelectionUI ui;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private TurnSelectionController controller;

    // 참조 데이터
    private BattleSequence sequence;

    // 현재 선택된 행동
    private BattleAction action;

    // 최소 배치 가능 Index
    private int minIndex;
    private int index;

    private void Awake()
    {
        sequence = BattleData.Instance.Sequence;
    }

    public void OpenSelection(SelectionData selectionData)
    {
        action = selectionData.action;

        // 타임라인 삽입 아이콘 활성화
        ActiveInsertIcon();

        // 배치 가능한 최소 위치 설정
        SetMinSibling(action);

        // 컨트롤러 설정(바로 전환 시 오류 발생)
        Invoke(nameof(UpdateController), 0.01f);
    }

    private void UpdateController()
    {
        actionManager.SetSubController(controller);
    }

    public void CloseSelection()
    {
        // 타임라인 삽입 아이콘 비활성화
        DeactiveInsertIcon();

        // 컨트롤러 없애기
        actionManager.SetSubController(null);
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

    /***************************************************************
    * [ 타임라인 선택 ]
    * 
    * 해당 액션을 어느 타임라인에 넣을 지 선택 처리
    ***************************************************************/

    private void ActiveInsertIcon()
    {
        Entity actor = action.actor;

        // 턴 선택 위치를 가시화한 선택 아이콘 활성화
        ui.SetActiveInsertIcon(true);

        // 센터 아이콘 마킹 비활성화
        timeline.CenterIcon.ClearMarking();

        // 삽입 아이콘 이미지를 현재 턴인 캐릭터의 초상화로 변경
        ui.SetInsertIconImage(actor.gameObject);
    }

    private void SetMinSibling(BattleAction action)
    {
        // 위치될 수 있는 최소 값
        minIndex = sequence.GetMinIndex(action);
        index = minIndex;

        // 아이콘 위치 설정
        ui.SetSiblingIcon(minIndex);

        // 삽입 아이콘을 중앙에 위치
        MoveIndex(minIndex);
    }

    private void DeactiveInsertIcon()
    {
        // 아이콘 비활성화
        ui.SetActiveInsertIcon(false);

        // 센터 아이콘 마킹 활성화
        timeline.CenterIcon.SetMarking();
    }

    public void MoveNext()
    {
        if (index < sequence.Sequence.Count)
        {
            MoveIndex(++index);
        }
    }

    public void MovePrev()
    {
        if (index > minIndex)
        {
            MoveIndex(--index);
        }
    }

    private void MoveIndex(int index)
    {
        // 타임라인 이동
        timeline.MoveTimelineAtIndex(index);

        // 해당 자리에 삽입 아이콘 위치
        ui.SetSiblingIcon(index);
    }

    public void InsertAction()
    {
        // 배치될 턴의 이전 행동의 턴 수 가져오기
        // (해당 턴 = 배치될 행동의 턴)
        BattleAction prevAction = sequence.GetTurnAction(index - 1);

        float selectTrun = (prevAction.remainTurn < action.remainTurn) ? action.remainTurn : prevAction.remainTurn;

        actionManager.SelectTurn(selectTrun, index);
    }
}