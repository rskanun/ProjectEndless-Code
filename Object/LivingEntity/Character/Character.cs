using UnityEngine;

public class Character : Entity
{
    [Header("참조 스크립트")]
    public BattleManager battleManager;
    public ActionManager actionManager;

    public void OnJoinBattle()
    {
        // 본인의 데이터를 파티데이터에서 가져옴
        CharacterData data = PartyData.Instance.GetCharacter(Name);

        // 데이터 덮어씌우기
        SkillList = data.Skills;
        Stat = data.Stat;

        // 오브젝트 활성화
        gameObject.SetActive(true);
    }

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public override void TakeTurn()
    {
        // 행동 선택창 열기
        actionManager.OnSelectAction(this);
    }

    public void OnAttackAction(AttackAction action)
    {
        battleManager.SetTurn(action);
    }

    public void OnSelectItem()
    {

    }
}