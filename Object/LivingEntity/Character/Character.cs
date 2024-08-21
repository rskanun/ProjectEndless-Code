using UnityEngine;

public class Character : Entity
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionSelection;

    // 전투 순서 데이터
    private BattleData battleData;

    private void Start()
    {
        battleData = BattleData.Instance;
    }

    public void OnJoinBattle()
    {
        // 본인의 데이터를 파티데이터에서 가져옴
        CharacterData data = PartyData.Instance.GetCharacter(Name);

        // 데이터 덮어씌우기
        Position = data.Position;
        AttackType = data.AttackType;
        SkillList = data.Skills;
        Stat = data.Stat;

        // HUD 업데이트
        InitHUD();

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
        if (battleData.IsInBattle == false)
        {
            // 전투가 끝났을 경우 행동을 하지 않고 종료
            EndTurn();

            return;
        }

        // 행동 선택창 열기
        actionSelection.OpenSelection(this);
    }

    public AttackAction CreateAttackAction()
    {
        AttackAction action = new AttackAction();

        action.remainTurn = 1.0f;  // 임시 턴수
        action.actor = this;

        return action;
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

    public void OnSelectAction(BattleAction action, int index)
    {
        // 행동 예약
        battleData.Sequence.AddTurn(action, index);

        // 턴 종료
        EndTurn();
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