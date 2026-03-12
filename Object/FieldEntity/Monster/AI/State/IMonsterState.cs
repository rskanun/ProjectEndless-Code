/* 몬스터 FSM
 * - Idle: 자유롭게 행동하는 상태
 * - Chase: 플레이어를 따라가는 상태
 * - Detect: 플레이어를 탐지하는 상태
 * - Attack: 플레이어를 향해 공격하는 상태
 */

public interface IMonsterState
{
    public void OnEnterState();
    public void OnAction(FSM fsm);
    public void OnTakeDamage(FSM fsm);
}