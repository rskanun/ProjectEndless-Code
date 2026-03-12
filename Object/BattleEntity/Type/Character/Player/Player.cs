using UnityEngine;

public class Player : Character
{

    public override sealed float GetCriticalChance(Entity target)
    {
        // 주인공의 경우 마력이 더 높으면,
        // 모든 공격이 크리티컬 값을 띄움
        if (target.FinalStats.MP < FinalStats.MP) return 1.0f;
        return base.GetCriticalChance(target);
    }
}