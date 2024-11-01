using System.Collections.Generic;
using System.Linq;

public class Belligerent : Personality
{
    public Belligerent() : base(PersonalityType.Belligerent) { }

    public override Dictionary<Entity, float> GetWeightData(List<Entity> targetList)
    {
        BattleSequence seq = CurrentBattleData.Instance.Sequence;
        Dictionary<Entity, float> weightData = new Dictionary<Entity, float>();

        // 공격 순서에 따른 가중치 값
        float actionWeight = 1.0f;

        // 가장 빨리 행동하는 순서대로 재정렬
        foreach (Entity target in targetList.OrderBy(entity => seq.GetSeqIndex(entity)))
        {
            // 가중치 초기값 설정
            weightData[target] = 0.0f;

            // 해당 엔티티의 예정된 행동 가져오기
            BattleAction action = seq.GetEntityAction(target);

            // 일반 공격이나 스킬을 쓸 예정이라면 가중치 증가
            if (action is AttackAction || action is SkillAction)
            {
                weightData[target] += actionWeight;

                // 다음 행동자는 가중치 증가량 감소
                actionWeight -= 0.1f;
            }

            // 전방에 위치한 적인 경우
            if (target.Position == BattlePosition.Front)
            {
                // 가중치 부여
                weightData[target] += 1.0f;
            }
        }

        return weightData;
    }
}