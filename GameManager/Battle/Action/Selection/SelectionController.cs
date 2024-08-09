using UnityEditor;
using UnityEngine;

public class SelectionController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private Timeline timeline;

    private bool isMoveKeyPressed;

    private void Awake()
    {
        ControlContext.Instance.SetState(this);
    }

    public void OnControlKeyPressed()
    {
        OnUndoKeyPressed();
        OnActionSelectKeyPressed();
        OnTimelineMoveKeyPressed();
        OnTimelineInsertKeyPressed();
    }

    public void OnUndoKeyPressed()
    {
        if (Input.GetButtonDown(KeyOption.Cancel))
        {
            actionManager.UndoSelection();
        }
    }

    public void OnActionSelectKeyPressed()
    {
        // 누른 키에 따른 행동 선택
        // ex) a키 -> 공격, s키 -> 스킬
    }

    public void OnTimelineMoveKeyPressed()
    {
        float h = Input.GetAxisRaw(KeyOption.AxisH);

        if (h != 0 && !isMoveKeyPressed)
        {
            if (h > 0)
            {
                timeline.MoveNext();
            }
            else if (h < 0)
            {
                timeline.MovePrev();
            }

            // 움직였으면 다음 움직임은 키에서 손을 땔 때까지 막기
            isMoveKeyPressed = true;
        }
        else if (h == 0)
        {
            // 키에서 손을 땠다면 다음 키를 누를 수 있도록 변경
            isMoveKeyPressed = false;
        }
    }

    public void OnTimelineInsertKeyPressed()
    {
        if (Input.GetButtonDown(KeyOption.Select))
        {
            TurnSelection selection = actionManager.openSelection as TurnSelection;
            if (selection != null)
            {
                selection.InsertAction();
            }
        }
    }
}