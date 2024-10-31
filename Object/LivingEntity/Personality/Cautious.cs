using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cautious : Personality
{
    private List<Entity> statusEffectCasters;
    private List<Entity> rangeAttackEntities;

    public Cautious() : base(PersonalityType.Cautious)
    {
        statusEffectCasters = new List<Entity>();
        rangeAttackEntities = new List<Entity>();
    }

    public override Dictionary<Entity, float> GetWeightData(List<Entity> targetList)
    {
        Dictionary<Entity, float> weightData = new Dictionary<Entity, float>();

        // 체력이 낮은 순으로 재정렬
        targetList.OrderBy(entity => entity.Stat.HP);

        float hpWeight = 1.0f; // 체력에 따른 가중치 값
        foreach (Entity target in targetList)
        {
            // 가중치 초기값 설정
            weightData[target] = 0.0f;

            // 해당 타겟이 상태 효과를 걸었던 적이 있는지
            if (statusEffectCasters.Contains(target))
            {
                // 걸었던 적이 있다면 가중치 부여
                weightData[target] += 4.0f;
            }

            // 해당 타겟이 원거리 공격을 했던 적이 있는지
            if (rangeAttackEntities.Contains(target))
            {
                // 걸었던 적이 있다면 가중치 부여
                weightData[target] += 2.0f;
            }

            // 체력이 낮은 적부터 높은 가중치 부여
            weightData[target] += hpWeight;

            // 체력 가중치 낮추기
            hpWeight -= 0.1f;
        }

        return weightData;
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

            // 기억상에 존재하지 않을 경우 추가
            if (!statusEffectCasters.Contains(attacker))
            {
                statusEffectCasters.Add(attacker);
                Debug.Log($"Memory: {attacker} is Ranger");
            }
        }
    }

    private bool IsRangeAttackSkill(Skill skill)
    {
        return skill is AttackSkill
            && skill.TargetType is not (TargetType.FrontMember or TargetType.FrontEnemy or TargetType.Self or TargetType.None);
    }
}