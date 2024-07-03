using UnityEngine;

public class Monster : Entity
{
    // 전투 순서 데이터
    private BattleSequence battleSeq;

    private void Awake()
    {
        battleSeq = BattleData.Instance.Sequence;
    }

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public override void TakeTurn()
    {
        // AI에 따른 행동 처리
        // 임시로 상시 대기 실행
        Invoke(nameof(OnWaitingAction), 2.0f);
    }

    public void OnWaitingAction()
    {
        WaitAction action = new WaitAction();

        action.actor = this;
        action.remainTurn = 5.0f / Stat.AGI;

        Debug.Log($"{Name} {action.remainTurn} 턴 뒤, 대기 행동 예약");
        battleSeq.AddTurn(action);

        // 행동 종료
        EndTurn();
    }

    public override void OnWaiting()
    {
        Debug.Log($"{Name} 대기");
        base.OnWaiting();
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public override void OnDamage(float damage, int targetMP)
    {
        base.OnDamage(damage, targetMP);
        Debug.Log($"{Name} {damage - Stat.DEF} Damage!!");
    }
}