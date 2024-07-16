using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform container;

    [Header("참조 스크립트")]
    [SerializeField] private SelectionManager manager;

    public Button CreateSelectButton(Vector2 pos)
    {
        GameObject selectButtonObj = Instantiate(targetPrefab, container);
        Button selectButton = selectButtonObj.GetComponent<Button>();

        // 버튼 위치값을 해당 엔티티의 중심으로 지정
        selectButtonObj.transform.position = pos;

        // 버튼 클릭 시 실행 이벤트 설정
        selectButton.onClick.AddListener(() => manager.OnSelect(selectButton));

        return selectButton;
    }
}