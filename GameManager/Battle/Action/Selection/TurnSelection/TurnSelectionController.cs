using UnityEngine;
using UnityEngine.EventSystems;

public class TurnSelectionController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private TurnSelection selection;

    private bool isMoveKeyPressed;

    public void OnControlKeyPressed()
    {
        OnTimelineMoveKeyPressed();
        OnTimelineInsertKeyPressed();
    }

    public void OnTimelineMoveKeyPressed()
    {
        float h = Input.GetAxisRaw(KeyOption.AxisH);

        if (h != 0 && !isMoveKeyPressed)
        {
            if (h > 0)
            {
                selection.MoveNext();
            }
            else if (h < 0)
            {
                selection.MovePrev();
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
            selection.InsertAction();
        }
    }
}