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

    private void Awake()
    {
        iconWidth = timelineIcon.GetComponent<RectTransform>().rect.width;
        spacing = groupComponent.spacing;
    }

    public void CenterIconAtIndex(int index)
    {
        // n번째 아이콘이 가운데에 위치하도록 위치 조정
        Vector2 startPos = new Vector2(-(iconWidth + 10f) / 2, container.localPosition.y);
        float moveDistance = (iconWidth + spacing) * index;

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
            // 삽입 아이콘 개수 만큼 뒤로 보내기
            iconObj.transform.SetSiblingIndex(index.Value + 2);
        }

        return icon;
    }

    /***************************************************************
    * [ 삽입 아이콘 관리 ]
    * 
    * 삽입 아이콘 활성화 설정 및 관리
    ***************************************************************/

    public void SetActiveInsertIcon(bool isActive)
    {
        insertIcon.SetActive(isActive);
    }

    public void SetSiblingInsertIcon(int index)
    {
        insertIcon.transform.SetSiblingIndex(index);
    }

    public void SetInsertIconImage(GameObject actor)
    {
        InsertIcon script = insertIcon.GetComponent<InsertIcon>();

        script.SetImage(actor);
    }
}