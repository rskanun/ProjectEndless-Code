using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private SkillSelectionUI ui;

    // 현재 스킬창 내 스킬 정보 오브젝트
    private List<GameObject> skillInfoList = new List<GameObject>();

    // 마지막 선택 버튼
    private GameObject lastSelected;

    public void OpenSelection(List<Skill> skills)
    {
        lastSelected = null;

        // 스킬창 열기
        ui.SetActiveWindow(true);

        // 스킬 정보 배치
        SetSkillsInfo(skills);

        // 초기 스킬 선택
        SelectLastButton();
    }

    public void CloseSelection()
    {
        // 스킬창 닫기
        ui.SetActiveWindow(false);
    }

    public void ReopenSelection()
    {
        // 이전 유지된 데이터를 기반으로 스킬창 열기
        ui.SetActiveWindow(true);

        // 초기 스킬 선택
        SelectLastButton();
    }

    public void UndoSelection()
    {
        // 스킬창 닫기
        CloseSelection();
    }

    /***************************************************************
    * [ UI 설정 ]
    * 
    * 스킬 선택창의 구성 UI 설정
    ***************************************************************/

    private void SetSkillsInfo(List<Skill> skills)
    {
        // 기존 스킬 정보 삭제
        ClearSkillList();

        // 새 스킬 정보 등록
        foreach (Skill skill in skills)
        {
            AddSkillInfo(skill);
        }
    }

    private void AddSkillInfo(Skill skill)
    {
        // 스킬 정보를 담은 오브젝트 생성
        GameObject skillInfoObj = ui.CreateSkillInfo(skill);

        // 스킬 정보 설정
        SkillInfo skillInfo = skillInfoObj.GetComponent<SkillInfo>();
        skillInfo.SetSkill(skill);

        // Hover 설정
        skillInfo.SetHoverHandler(() => ui.SetDescription(skill.Description));

        // 버튼 클릭 설정
        skillInfo.SetClickHandler(() =>
        {
            actionSelection.OnSelectSkill(skill);
            lastSelected = skillInfoObj;
        });

        // 파괴를 위해 리스트에 추가
        skillInfoList.Add(skillInfoObj);
    }

    private void ClearSkillList()
    {
        foreach (GameObject skillInfoObj in skillInfoList)
        {
            // 스킬 정보 오브젝트 삭제
            Destroy(skillInfoObj);
        }

        // 리스트 초기화
        skillInfoList.Clear();
    }

    private void SelectLastButton()
    {
        if (skillInfoList.Count > 0)
        {
            // 마지막으로 선택한 버튼이 없을 경우 첫버튼 선택
            if (lastSelected == null)
                lastSelected = skillInfoList[0];

            // 버튼 선택
            EventSystem.current.SetSelectedGameObject(lastSelected);

            // 설명 설정
            SkillInfo skill = lastSelected.GetComponent<SkillInfo>();
            string description = skill.GetSkill().Description;

            ui.SetDescription(description);
        }
    }
}