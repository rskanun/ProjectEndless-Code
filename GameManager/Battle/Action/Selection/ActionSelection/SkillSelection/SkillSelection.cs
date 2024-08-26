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

    public void OpenSelection(Character actor)
    {
        lastSelected = null;

        // 스킬창 열기
        ui.SetActiveWindow(true);

        // 스킬 정보 배치
        InitSkillsInfo(actor.SkillList, actor.Stat.SP);

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

    private void InitSkillsInfo(List<Skill> skills, int remainSP)
    {
        // 기존 스킬 정보 삭제
        ClearSkillList();

        // 새 스킬 정보 등록
        foreach (Skill skill in skills)
        {
            // 스킬 정보 오브젝트 생성
            GameObject skillInfoObj = CreateSkillInfoObject(skill, remainSP);

            // 오브젝트 리스트에 추가
            skillInfoList.Add(skillInfoObj);
        }
    }

    private GameObject CreateSkillInfoObject(Skill skill, int remainSP)
    {
        // 스킬 정보를 담은 오브젝트 생성
        GameObject skillInfoObj = ui.CreateSkillInfo(skill);

        // 스킬 정보 설정
        SkillInfo skillInfo = skillInfoObj.GetComponent<SkillInfo>();
        skillInfo.SetSkill(skill);

        // hover 설정
        skillInfo.SetHoverHandler(() => ui.SetDescription(skill.Description));

        // 버튼 클릭 설정
        skillInfo.SetClickHandler(() =>
        {
            OnSkillClicked(skill, skillInfoObj);
        });

        // 스킬 사용 여부 설정
        skillInfo.SetUsable(skill.CostSP <= remainSP);

        return skillInfoObj;
    }

    private void OnSkillClicked(Skill skill, GameObject skillInfoObj)
    {
        actionSelection.OnSelectSkill(skill);
        lastSelected = skillInfoObj;
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
        if (skillInfoList.Count == 0) return;

        if (lastSelected == null)
        {
            // 이전에 선택한 버튼이 없는 경우 선택 가능한 첫번째 요소를 선택
            lastSelected = GetFirstUsableItem();
        }

        // 버튼 선택
        EventSystem.current.SetSelectedGameObject(lastSelected);

        // 스킬 설명 설정
        UpdateDescription(lastSelected);
    }

    private GameObject GetFirstUsableItem()
    {
        // 첫번째 선택 요소 찾기
        foreach (GameObject skillInfoObj in skillInfoList)
        {
            SkillInfo skillInfo = skillInfoObj.GetComponent<SkillInfo>();

            // 해당 스킬을 사용가능한 경우
            if (skillInfo.IsUsable())
            {
                // 해당 스킬을 첫번째 선택 요소로 반환
                return skillInfoObj;
            }
        }

        return null;
    }

    private void UpdateDescription(GameObject selectedItem)
    {
        SkillInfo skill = selectedItem.GetComponent<SkillInfo>();
        ui.SetDescription(skill.GetSkill().Description);
    }
}