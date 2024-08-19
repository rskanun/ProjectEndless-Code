using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour, ISubActionSelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection actionSelection;

    [Header("스킬창 구성요소")]
    public GameObject selectionWindow;
    public GameObject skillInfoPrefab;
    public Transform skillContainer;
    public TextMeshProUGUI descriptionText;

    [Header("스킬 아이콘")]
    public Sprite attackSkillIcon;

    // 현재 스킬창 내 스킬 정보 오브젝트
    private List<GameObject> skillInfoList = new List<GameObject>();

    public void OpenSkillSelection(List<Skill> skills)
    {
        // 스킬창 열기
        SetActiveWindow(true);

        // 스킬 정보 배치
        SetSkillsInfo(skills);
    }

    public void CloseSubSelection()
    {
        // 스킬창 닫기
        SetActiveWindow(false);
    }

    public void ReopenSubSelection()
    {
        // 스킬 정보 그대로 스킬창 열기
        SetActiveWindow(true);
    }

    /***************************************************************
    * [ UI 설정 ]
    * 
    * 스킬 선택창의 구성 UI 설정
    ***************************************************************/

    private void SetActiveWindow(bool isActive)
    {
        selectionWindow.SetActive(isActive);
    }

    private void SetSkillsInfo(List<Skill> skills)
    {
        // 기존 스킬 정보 삭제
        ClearSkillList();

        // 새 스킬 정보 등록
        foreach (Skill skill in skills)
        {
            AddSkillInfo(skill);
        }

        // 스킬 정보창 초기값 설정
        if (skillInfoList.Count > 0)
        {
            // 첫번째 스킬을 초기 선택 스킬로 설정
            EventSystem.current.SetSelectedGameObject(skillInfoList[0]);

            // 해당 스킬 설명 설정
            SetDescription(skills[0]);
        }
    }

    private void ClearSkillList()
    {
        foreach(GameObject skillInfoObj in skillInfoList)
        {
            // 스킬 정보 오브젝트 삭제
            Destroy(skillInfoObj);
        }

        // 리스트 초기화
        skillInfoList.Clear();
    }

    private void AddSkillInfo(Skill skill)
    {
        GameObject skillInfoObj = Instantiate(skillInfoPrefab, skillContainer);

        // 스킬 정보 설정
        SkillInfo skillInfo = skillInfoObj.GetComponent<SkillInfo>();
        skillInfo.SetSkill(skill);

        // Hover 설정
        skillInfo.SetHoverHandler(() => SetDescription(skill));

        // 버튼 클릭 설정
        Button button = skillInfoObj.GetComponent<Button>();
        button.onClick.AddListener(() => actionSelection.OnSelectSkill(skill));

        // 스킬 정보 목록에 추가
        skillInfoList.Add(skillInfoObj);
    }

    public void SetDescription(Skill skill)
    {
        descriptionText.text = skill.Description;
    }
}