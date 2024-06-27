using UnityEngine;

public class Monster : Entity
{
    [Header("참조 스크립트")]
    public BattleManager battleManager;

    public override void TakeTurn()
    {
        // AI에 따른 행동 처리
        // 임시로 상시 대기 실행
        Invoke(nameof(OnWaiting), 2.0f);
    }

    public override void OnWaiting()
    {
        // 임시 대기 행동
        Debug.Log($"{Name} 대기");

        WaitAction action = new WaitAction();
        action.remainTurn = 5.0f / Stat.AGI;

        battleManager.SetTurn(action);
    }
}