using UnityEngine;
using UnityEngine.UI;

public class TimelineUI : MonoBehaviour
{
    public HorizontalLayoutGroup groupComponent;
    public RectTransform container;
    public GameObject timelineIcon;

    // 타임라인 위치 데이터
    private float iconWidth;
    private float spacing;

    private void Awake()
    {
        iconWidth = timelineIcon.GetComponent<RectTransform>().rect.width;
        spacing = groupComponent.spacing;
    }

    public void SetPos(int iconIndex)
    {
        // n번째 아이콘이 가운데에 위치하도록 위치 조정
        Vector2 startPos = new Vector2(-iconWidth / 2, container.localPosition.y);
        float moveDistance = (iconWidth + spacing) * iconIndex;

        container.localPosition = new Vector2(startPos.x - moveDistance, startPos.y);
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
}