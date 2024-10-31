using System.Collections.Generic;
using System.Linq;

public class Brave : Personality
{
    public Brave() : base(PersonalityType.Brave) { }

    public override Dictionary<Entity, float> GetWeightData(List<Entity> targetList)
    {
        BattleSequence seq = CurrentBattleData.Instance.Sequence;
        Dictionary<Entity, float> weightData = new Dictionary<Entity, float>();

        // 힘과 마력의 합이 높은 순으로 재정렬
        targetList.OrderBy(entity => entity.Stat.STR + entity.Stat.MaxMP);

        float atkWeight = 2.0f; // 공격력에 따른 가중치 값
        foreach (Entity target in targetList)
        {
            // 가중치 초기값 설정
            weightData[target] = 0.0f;

            // 공격력이 높은 적에게 높은 가중치 부여
            weightData[target] += atkWeight;

            // 전방에 위치한 적인 경우
            if (target.Position == BattlePosition.Front)
            {
                // 가중치 부여
                weightData[target] += 2.0f;
            }

            // 공격력 가중치 낮추기
            atkWeight -= 0.1f;
        }

        // 가장 빨리 행동하는 순서대로 재정렬
        targetList.OrderBy(entity => seq.GetEntityAction(entity));

        float actionWeight = 0.5f; // 행동에 따른 가중치 값
        foreach (Entity target in targetList)
        {
            // 해당 엔티티의 예정된 행동 가져오기
            BattleAction action = seq.GetEntityAction(target);

            // 일반 공격이나 스킬을 쓸 예정이라면 가중치 증가
            if (action is AttackAction || action is SkillAction)
            {
                weightData[target] += actionWeight;

                // 다음 행동자는 가중치 증가량 감소
                actionWeight -= 0.1f;
            }
        }

        return weightData;
    }
}