using UnityEngine;

public class TargetSelectionUI : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform container;

    [Header("참조 스크립트")]
    [SerializeField] private TargetSelection manager;

    public OldTargetSelectButton CreateSelectButton(Entity target, Vector2 pos)
    {
        GameObject selectButtonObj = Instantiate(targetPrefab, container);
        OldTargetSelectButton selection = selectButtonObj.GetComponent<OldTargetSelectButton>();

        // 버튼 위치값을 해당 엔티티의 중심으로 지정
        selectButtonObj.transform.position = pos;

        // 버튼 클릭 시 실행 이벤트 설정
        selection.SetTarget(target);
        selection.SetListener(() => manager.OnSelect(target));

        return selection;
    }
}