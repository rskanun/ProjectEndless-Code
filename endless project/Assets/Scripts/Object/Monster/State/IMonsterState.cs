/* 몬스터 FSM
 * - Idle: 자유롭게 행동하는 상태
 * - Chase: 플레이어를 따라가는 상태
 * - Run: 플레이어로부터 멀어지는 상태
 * - Attack: 플레이어를 향해 공격하는 상태
 * - Help: 플레이어를 향해 이로운 효과를 주는 상태
 * - Sleep: 잠든 상태
 * - Faint: 기절 상태
 */

public interface IMonsterState
{
    public void OnEnterState();
    public void OnAction(FSM fsm);
    public void OnTakeDamage(FSM fsm);
}