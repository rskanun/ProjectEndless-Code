using UnityEngine;

public class TurnSelectionUI : MonoBehaviour
{
    public GameObject insertIcon;

    public void SetActiveInsertIcon(bool isActive)
    {
        insertIcon.SetActive(isActive);
    }

    public bool IsActiveInsertIcon()
    {
        return insertIcon.activeSelf;
    }

    public void SetInsertIconImage(GameObject actor)
    {
        InsertIcon script = insertIcon.GetComponent<InsertIcon>();

        script.SetImage(actor);
    }

    public void SetActiveInsertMode(bool isActive)
    {
        isInsertMode = isActive;

        // 삽입 아이콘 활성화 설정
        ui.SetActiveInsertIcon(isActive);

        // 활성화일 경우 아이콘 이미지 변경
        if (isActive)
        {
            Entity curTurnChr = timelines[0].Action.actor;
            GameObject chrObj = curTurnChr.gameObject;

            // 아이콘 이미지 변경
            ui.SetInsertIconImage(chrObj);
        }
    }

    public void MoveToPrev()
    {
        if (curMiddleIndex > 0)
        {
            MoveTimeline(curMiddleIndex - 1);
        }
    }

    public void MoveToNext()
    {
        if (curMiddleIndex < timelines.Count - 1)
        {
            MoveTimeline(curMiddleIndex + 1);
        }
    }
}