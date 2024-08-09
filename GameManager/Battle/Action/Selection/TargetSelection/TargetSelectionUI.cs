using UnityEngine;

public class TargetSelectionUI : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform container;

    // 연결할 버튼
    private TargetSelectButton head;
    private TargetSelectButton tail;

    [Header("참조 스크립트")]
    [SerializeField] private TargetSelection manager;

    private static int count = 0;
    public TargetSelectButton CreateSelectButton(Entity target, Vector2 pos)
    {
        GameObject selectButtonObj = Instantiate(targetPrefab, container);
        TargetSelectButton selectButton = selectButtonObj.GetComponent<TargetSelectButton>();

        // 버튼 위치값을 해당 엔티티의 중심으로 지정
        selectButtonObj.transform.position = pos;

        // 버튼 클릭 시 실행 이벤트 설정
        selectButton.targetEntity = target;
        selectButton.AddListener(() => manager.OnSelectOne(target));

        // 버튼 연결
        SetButtonLinked(selectButton);

        return selectButton;
    }

    private void SetButtonLinked(TargetSelectButton button)
    {
        if (head == null && tail == null)
        {
            // 연결이 하나도 없는 경우 새 연결 생성
            head = button;
            tail = button;

            button.PrevButton = button;
            button.NextButton = button;
        }
        else
        {
            // 새 버튼은 항상 맨 뒤에 붙음
            button.PrevButton = tail;
            button.NextButton = head;

            tail.NextButton = button;
            head.PrevButton = button;

            tail = button;
        }
    }
}