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

    public void OpenSelection(List<Skill> skills)
    {
        // 스킬창 열기
        ui.SetActiveWindow(true);

        // 스킬 정보 배치
        SetSkillsInfo(skills);
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

        // 스킬 정보창 초기값 설정
        if (skillInfoList.Count > 0)
        {
            // 첫번째 스킬을 초기 선택 스킬로 설정
            EventSystem.current.SetSelectedGameObject(skillInfoList[0]);

            // 해당 스킬 설명 설정
            ui.SetDescription(skills[0]);
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
        skillInfo.SetHoverHandler(() => ui.SetDescription(skill));

        // 버튼 클릭 설정
        skillInfo.SetClickHandler(() => actionSelection.OnSelectSkill(skill));

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
}