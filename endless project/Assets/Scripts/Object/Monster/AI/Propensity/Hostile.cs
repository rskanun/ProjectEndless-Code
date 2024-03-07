using UnityEngine;

public class Hostile : Propensity
{
    public override void OnIdleAction(FSM fsm)
    {
        // 적대적 성향일 경우 플레이어 탐지
        Vector3 playerPos = monster.DetectPlayer();
        if (playerPos != monster.transform.position)
        {
            // 플레이어 추적 상태로 변경
            fsm.SetState(new ChaseState(monster));
        }
    }
}