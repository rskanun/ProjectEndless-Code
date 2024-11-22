using UnityEngine;

public class Player : Character
{
    [SerializeField] private AssistAttackManager assistManager;

    public override sealed float GetCriticalChance(Entity target)
    {
        // 주인공의 경우 마력이 더 높으면,
        // 모든 공격이 크리티컬 값을 띄움
        if (target.Stat.MP < Stat.MP) return 1.0f;
        return base.GetCriticalChance(target);
    }

    protected override void OnParryAction()
    {
        battleData.IsUsedParry = true;
    }

    protected override void OnDodgeAction()
    {
        battleData.IsUsedDodge = true;
    }

    public override void OnParrying(Entity attacker)
    {
        // 플레이어가 패링에 성공했을 경우 추가타를 넣을 대상 선택
        assistManager.OnSelectExtraAttacker(attacker);
    }
}