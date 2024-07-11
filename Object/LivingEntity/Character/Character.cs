using UnityEngine;

public class Character : Entity
{
    [Header("참조 스크립트")]
    public ActionManager actionManager;

    // 전투 순서 데이터
    private BattleSequence battleSeq;

    private void Start()
    {
        battleSeq = BattleData.Instance.Sequence;
    }

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

    public void OnAttackAction(Entity target)
    {
        AttackAction action = new AttackAction();

        action.remainTurn = 1.0f;  // 임시 턴수
        action.actor = this;
        action.target = target;

        // 행동 예약
        battleSeq.AddTurn(action);

        // 턴 종료
        EndTurn();
    }

    public override void OnAttack(Entity target)
    {
        // 공격 행동
        if (target != null)
        {
            target.OnDamage(Stat.STR, Stat.MP);
            Debug.Log($"{Name} Attack {target.Name}!!");
        }

        // 공격하려는 대상이 없는 경우 공격가능한 다른 대상을 타겟으로 공격
        // 캐릭터의 성격마다 우선순위로 선택하는 타겟이 다름
    }

    public void OnSelectItem()
    {

    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public override void OnDead()
    {
        throw new System.NotImplementedException();
    }

    public override void OnManaShort()
    {
        throw new System.NotImplementedException();
    }
}