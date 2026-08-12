using System.Collections.Generic;
using UnityEngine;

public class Belligerent : Personality
{
    // 재사용 인스턴스
    private Dictionary<Entity, float> weightData = new(4);
    private List<Entity> sortedList = new(4);

    public Belligerent() : base(PersonalityType.Belligerent) { }

    protected override Dictionary<Entity, float> GetWeightData(List<Entity> targetList)
    {
        BattleSequence seq = BattleData.Instance.Sequence;

        // 재사용 인스턴스 초기화
        weightData.Clear();
        sortedList.Clear();

        // 타겟이 없는 경우 빈 값 리턴
        if (targetList == null || targetList.Count == 0)
        {
            return weightData;
        }

        // 리스트 복사(오염 방지)
        sortedList.AddRange(targetList);

        // 가장 먼저 행동하는 순서대로 재정렬
        sortedList.Sort(CompareBySeqIndex);

        // 공격 순서에 따른 가중치 값
        float actionWeight = 1.0f;

        foreach (Entity target in sortedList)
        {
            // 중복 방지
            if (weightData.ContainsKey(target)) continue;

            // 가중치 초기값 설정
            weightData[target] = 0.0f;

            // 해당 엔티티의 예정된 행동 가져오기
            var action = seq.GetEntityAction(target);

            // 일반 공격이나 스킬을 쓸 예정이라면 가중치 증가
            if (action is AttackAction || action is SkillAction)
            {
                weightData[target] += actionWeight;

                // 다음 행동자는 가중치 증가량 감소(최소 0)
                actionWeight = Mathf.Max(0.0f, actionWeight - 0.1f);
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

    private int CompareBySeqIndex(Entity a, Entity b)
    {
        var seq = BattleData.Instance.Sequence;
        var idxA = seq.GetSeqIndex(a);
        var idxB = seq.GetSeqIndex(b);

        return idxA.CompareTo(idxB);
    }
}