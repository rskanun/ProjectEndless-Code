using UnityEngine;

public class DetectState : IMonsterState
{
    private Monster monster;

    private float duration;

    public DetectState(Monster monster)
    {
        this.monster = monster;
    }

    public void OnEnterState()
    {
        duration = Random.Range(1f, 5f);
    }

    public void OnAction(FSM fsm)
    {
        if (duration > 0f)
        {
            Vector3 playerPos = DetectPlayer();
            if (playerPos != monster.transform.position)
            {
                monster.Personality.OnDetectedPlayer();
            }

            duration -= Time.deltaTime;
        }
        else fsm.SetState(new IdleState(monster));
    }

    private Vector3 DetectPlayer()
    {
        // 플레이어 방향으로 고개를 돌려 확인
        monster.RotateTo(ReadOnlyPlayerData.Instance.Position);

        return monster.DetectPlayer();
    }

    public void OnTakeDamage(FSM fsm) { }
}