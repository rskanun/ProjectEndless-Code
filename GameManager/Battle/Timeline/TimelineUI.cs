using UnityEngine;
using UnityEngine.UI;

public class TimelineUI : MonoBehaviour
{
    public HorizontalLayoutGroup groupComponent;
    public RectTransform container;
    public GameObject timelineIcon;
    public GameObject insertIcon;

    // 타임라인 위치 데이터
    private float iconWidth;
    private float spacing;

    public void InitPosData()
    {
        iconWidth = timelineIcon.GetComponent<RectTransform>().rect.width;
        spacing = groupComponent.spacing;
    }

    public void ResetPos(int iconCount)
    {
        float startPosX = (iconWidth + spacing) / 2 * (iconCount - 1);
        Vector2 startPos = new Vector2(startPosX, container.localPosition.y);

        container.localPosition = startPos;
    }

    /***************************************************************
    * [ 타임라인 관리 ]
    * 
    * 타임라인 아이콘 생성 및 관리
    ***************************************************************/

    public TimelineIcon CreateTimelineIcon(BattleAction action, int? index = null)
    {
        GameObject iconObj = Instantiate(timelineIcon, container);
        TimelineIcon icon = iconObj.GetComponent<TimelineIcon>();

        // 타임라인 지정
        icon.SetTimeline(action);

        // 위치 지정
        if (index.HasValue)
        {
            iconObj.transform.SetSiblingIndex(index.Value + 1);
        }

        return icon;
    }

    public void SetActiveInsertIcon(bool isActive)
    {
        insertIcon.SetActive(isActive);
    }

    public void SetInsertIconImage(GameObject actor)
    {
        InsertIcon script = insertIcon.GetComponent<InsertIcon>();

        script.SetImage(actor);
    }
}