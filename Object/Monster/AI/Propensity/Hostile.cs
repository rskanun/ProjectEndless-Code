using UnityEngine;

public class Hostile : Propensity
{
    public Hostile(Monster monster) : base(monster) { }

    public override void OnIdleAction(FSM fsm)
    {
        // 적대적 성향일 경우 플레이어 탐지
        Vector3 playerPos = monster.DetectPlayer();
        if (playerPos != monster.transform.position)
        {
            // 성격에 따른 상태 변경
            monster.Personality.OnDetectedPlayer();
        }
    }
}