using UnityEngine;

public class RunState : IMonsterState
{
    private Monster monster;

    public RunState(Monster monster)
    {
        this.monster = monster;
    }

    public void OnEnterState() { }

    public void OnAction(FSM fsm)
    {
        Vector3 playerPos = DetectPlayer();
        if (playerPos != monster.transform.position)
        {
            // 플레이어로부터 도망
            Vector2 moveVec = monster.transform.position - playerPos;

            monster.MoveTo(moveVec);
        }
        else fsm.SetState(new DetectState(monster));
    }

    private Vector3 DetectPlayer()
    {
        // 플레이어 방향으로 고개를 돌려 확인
        monster.RotateTo(ReadOnlyPlayerData.Instance.Position);

        return monster.DetectPlayer();
    }

    public void OnTakeDamage(FSM fsm) { }
}