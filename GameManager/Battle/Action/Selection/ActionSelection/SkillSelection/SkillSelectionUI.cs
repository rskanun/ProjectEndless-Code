using TMPro;
using UnityEngine;

public class SkillSelectionUI : MonoBehaviour
{
    [Header("스킬창 구성요소")]
    public GameObject selectionWindow;
    public GameObject skillInfoPrefab;
    public Transform skillContainer;
    public TextMeshProUGUI descriptionText;

    public void SetActiveWindow(bool isActive)
    {
        selectionWindow.SetActive(isActive);
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }

    public GameObject CreateSkillInfo(Skill skill)
    {
        GameObject skillInfoObj = Instantiate(skillInfoPrefab, skillContainer);

        // 생성된 오브젝트 리턴
        return skillInfoObj;
    }
}