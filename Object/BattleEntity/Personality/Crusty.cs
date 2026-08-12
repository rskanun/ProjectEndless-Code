using System.Collections.Generic;
using UnityEngine;

public class Crusty : Personality
{
    // 재사용 인스턴스
    private Dictionary<Entity, float> weightData = new(4);
    private List<Entity> sortedList = new(4);

    private HashSet<Entity> statusEffectCasters;
    private HashSet<Entity> rangeAttackEntities;

    public Crusty() : base(PersonalityType.Crusty)
    {
        statusEffectCasters = new(4);
        rangeAttackEntities = new(4);
    }

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
            // 가중치 초기값 설정
            weightData[target] = 0.0f;

            // 해당 엔티티의 예정된 행동 가져오기
            BattleAction action = seq.GetEntityAction(target);

            // 일반 공격이나 스킬을 쓸 예정이라면 가중치 증가
            if (action is AttackAction || action is SkillAction)
            {
                weightData[target] += actionWeight;

                // 다음 행동자는 가중치 증가량 감소(최소 0)
                actionWeight = Mathf.Max(0.0f, actionWeight - 0.1f);
            }

            // 해당 타겟이 원거리 공격을 했던 적이 있는지
            if (rangeAttackEntities.Contains(target))
            {
                // 걸었던 적이 있다면 가중치 부여
                weightData[target] += 4.0f;
            }

            // 해당 타겟이 상태 효과를 걸었던 적이 있는지
            if (statusEffectCasters.Contains(target))
            {
                // 걸었던 적이 있다면 가중치 부여
                weightData[target] += 2.0f;
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

    protected override void GatherCurTurnAction(BattleAction action)
    {
        // 상태이상 스킬 사용자인지 판별 후 메모리에 추가
        AddStatusEffectCaster(action);

        // 원거리 공격을 할 수 있는지 판별 후 메모리에 추가
        AddRangeAttacker(action);
    }

    private void AddStatusEffectCaster(BattleAction action)
    {
        // 해당 행동에서 스킬을 사용하고 상태 이상 스킬인 경우
        if (action is SkillAction skillAction && skillAction.castSkill is EffectSkill)
        {
            Entity caster = action.actor;

            // 기억상에 존재하지 않을 경우 추가
            if (!statusEffectCasters.Contains(caster))
            {
                statusEffectCasters.Add(caster);
                Debug.Log($"Memory: {caster} is Status Effect Caster");
            }
        }
    }

    private void AddRangeAttacker(BattleAction action)
    {
        // 행동이 원거리 일반 공격이거나, 원거리 공격 스킬을 사용하는 경우
        bool isRangedAttackAction = action is AttackAction && action.actor.AttackType == AttackType.Ranged;
        bool isRangedSkillAction = action is SkillAction skillAction && skillAction.castSkill is AttackSkill && IsRangeAttackSkill(skillAction.castSkill);

        if (isRangedAttackAction || isRangedSkillAction)
        {
            Entity attacker = action.actor;

            // 리스트에 존재하지 않는 경우에만 추가
            if (!statusEffectCasters.Contains(attacker))
            {
                statusEffectCasters.Add(attacker);
                Debug.Log($"Memory: {attacker.Name} is Ranger");
            }
        }
    }

    private bool IsRangeAttackSkill(Skill skill)
    {
        return skill is AttackSkill
            && skill.TargetType is not (TargetType.FrontMember or TargetType.FrontEnemy or TargetType.Self or TargetType.None);
    }
}