using System.Collections.Generic;

public enum EntityState
{
    // 행동에 영향을 끼치는 상태 목록

    Stun,   // 행동 불가 상태
    Stagger,    // 행동이 흐트러진 상태 -> 무조건 치명타 + 공격 방어 X
    Dodge,  // 공격 회피 상태
}

public class EntityStateManager
{
    private List<EntityState> entityStates;

    public EntityStateManager()
    {
        entityStates = new List<EntityState>();
    }

    public void Add(EntityState state)
    {
        // 상태 중첩 방지
        if (HasState(state)) return;

        entityStates.Add(state);
    }

    public void Remove(EntityState state)
    {
        entityStates.Remove(state);
    }

    public bool HasState(EntityState state)
    {
        return entityStates.Contains(state);
    }
}