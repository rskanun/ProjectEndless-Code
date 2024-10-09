using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Surpporter : Monster
{
    /***************************************************************
    * [ 행동 패턴 ]
    * 
    * 서포터 포지션 일반 몬스터의 공격 패턴 AI
    ***************************************************************/

    protected override void SelectAction()
    {
        // 일반 공격과 공격 스킬 위주로 턴 탐색
        BattleAction action = GetMinAttackTurn();

        if (action != null)
        {
            // 가장 가까운 턴이 아군 턴인 경우의 행동
            if (action.actor is Monster)
            {
                Entity target = action.actor;

                // 공격 관련 버프 스킬 탐색
                List<Skill> effects = FindUsableAttackBuffSkills(target);

                // 사용할 만한 공격 관련 버프 스킬이 없는 경우 방어 관련 스킬 탐색
                if (effects == null || effects.Count <= 0)
                {
                    effects = FindUsableDefenseBuffSkills(target);
                }

                // 사용할 만한 방어 관련 버프 스킬이 없는 경우
                if (effects == null || effects.Count <= 0)
                {
                    // 가장 가까운 턴의 적 탐색
                    target = GetMinAttackTurn<Character>().actor;

                    // 디버프 스킬 탐색
                    effects = FindUsableDebuffSkills(target);
                }

                // 사용할 만한 디버프 스킬이 없는 경우
                if (effects == null || effects.Count <= 0)
                {
                    // 일반 공격 실행
                    SelectAttack();
                    return;
                }

                // 사용 가능한 상태이상 스킬 중 하나를 랜덤으로 뽑아 사용
                Skill castSkill = GetRandomSkill(effects);
                SelectSkill(castSkill, target);

                return;
            }

            // 가장 가까운 턴이 적 턴인 경우의 행동
            if (action.actor is Character)
            {
                Entity target = action.actor;

                // 디버프 스킬 탐색
                List<Skill> effects = FindUsableDebuffSkills(target);

                // 사용할 만한 디버프 스킬이 없는 경우 방어 관련 스킬 탐색
                if (effects == null || effects.Count <= 0)
                {
                    // 가장 가까운 턴의 아군 탐색
                    target = GetMinAttackTurn<Monster>().actor;

                    // 방어 관련 버프 스킬 탐색
                    effects = FindUsableDefenseBuffSkills(target);
                }

                // 사용할 만한 방어 관련 버프 스킬이 없는 경우
                if (effects == null || effects.Count <= 0)
                {
                    // 일반 공격 실행
                    SelectAttack();
                    return;
                }

                // 사용 가능한 상태이상 스킬 중 하나를 랜덤으로 뽑아 사용
                Skill castSkill = GetRandomSkill(effects);
                SelectSkill(castSkill, target);

                return;
            }
        }

        // 누구의 턴도 아닌 경우 일반 공격 실행
        SelectAttack();
    }

    private BattleAction GetMinAttackTurn()
    {
        return GetMinAttackTurn<Entity>();
    }

    private BattleAction GetMinAttackTurn<T>() where T : Entity
    {
        foreach (BattleAction action in battleSeq.Sequence)
        {
            if (action.actor is T)
            {
                // 일반 공격일 경우 해당 행동 리턴
                if (action is AttackAction)
                {
                    return action;
                }

                // 공격 스킬일 경우 해당 행동 리턴
                if (action is SkillAction skillAction)
                {
                    if (skillAction.castSkill is AttackSkill)
                    {
                        return action;
                    }
                }
            }
        }

        // 현재 등록된 행동 안에서 공격 행동은 없음
        return null;
    }

    private List<Skill> FindUsableAttackBuffSkills(Entity target)
    {
        // 소유 중인 공격 관련 버프 목록
        List<EffectSkill> attackBuffs = GetEffectSkills(BuffType.AttackBuff);

        // 사용 가능한 범위 버프 스킬 탐색
        List<EffectSkill> areaBuffs = GetMultiTargetEffectSkills(attackBuffs);
        List<Skill> usableAreaBuffs = FindUsableEffectSkills(areaBuffs, target, target);

        if (usableAreaBuffs == null || usableAreaBuffs.Count <= 0)
        {
            // 사용 가능한 범위 버프 스킬이 없는 경우 단일 버프 탐색
            List<EffectSkill> singleBuffs = GetSingleTargetEffectSkills(attackBuffs);
            List<Skill> usableSingleBuffs = FindUsableEffectSkills(singleBuffs, target, target);

            return usableSingleBuffs;
        }

        // 공격 관련 버프 중 사용 가능한 스킬
        return usableAreaBuffs;
    }

    private List<Skill> FindUsableDefenseBuffSkills(Entity target, Entity attacker)
    {
        List<EffectSkill> defenseBuffs = GetEffectSkills(BuffType.DefenseBuff);

        return FindUsableEffectSkills(defenseBuffs, target);
    }

    private List<EffectSkill> GetEffectSkills(BuffType type)
    {
        return SkillList.OfType<EffectSkill>()
            .Where(skill => skill.Effect is Buff buff && buff.Type == type)
            .ToList();
    }

    private List<Skill> FindUsableDebuffSkills(Entity target)
    {
        List<EffectSkill> debuffs = SkillList.OfType<EffectSkill>()
            .Where(skill => skill.Effect is Debuff buff)
            .ToList();

        return FindUsableEffectSkills(debuffs, target);
    }

    private List<EffectSkill> GetMultiTargetEffectSkills(List<EffectSkill> skillList)
    {
        return skillList
            .Where(skill => skill.TargetType == TargetType.EnemyParty
                || skill.TargetType == TargetType.PlayerParty)
            .ToList();
    }

    private List<EffectSkill> GetSingleTargetEffectSkills(List<EffectSkill> skillList)
    {
        return skillList
            .Where(skill => skill.TargetType != TargetType.EnemyParty
                && skill.TargetType != TargetType.PlayerParty)
            .ToList();
    }

    private List<Skill> FindUsableEffectSkills(List<EffectSkill> skillList, Entity target, Entity attacker)
    {
        return skillList
            .Where(skill => IsUsableEffectSkill(skill, target, attacker))
            .Cast<Skill>()
            .ToList();
    }

    private bool IsUsableEffectSkill(EffectSkill skill, Entity target, Entity attacker)
    {
        // 사용 가능할 정도의 SP 소지 여부 확인
        if (skill.CostSP <= Stat.SP) return false;

        // attacker의 턴까지 타겟의 상태이상 소유 여부 확인
        float effectDuration = target.GetEffectDuration(skill.Effect);
        float attackerActionTurn = battleSeq.GetTurnAction(attacker).remainTurn;
        if (effectDuration > attackerActionTurn) return false;

        // attacker의 행동 순서보다 상태이상 스킬의 발동 순서가 더 앞인지 여부 확인
        int attackerActionIndex = battleSeq.GetActionMinSeq(attacker);
        int casterActionIndex = battleSeq.GetActionMinSeq(this, skill.CostTurn);
        if (attackerActionIndex < casterActionIndex) return false;

        // 상태이상 지속시간 안에 attacker의 행동이 실행되는지 여부 확인
        float effectEndTurn = skill.CostTurn + skill.Effect.Duration;
        if (effectEndTurn <= attackerActionTurn) return false;

        // 모든 조건을 만족하면 해당 스킬을 사용할 수 있음
        return true;
    }

    private void SelectAttack()
    {
        // 성격(우선 순위)에 따른 타겟 선택
        Entity target = Personality.SelectTarget();

        // 일반 공격 실행
        SelectAttack(target);
    }

    private Skill GetRandomSkill(List<Skill> skillList)
    {
        if (skillList == null || skillList.Count <= 0)
        {
            // 스킬이 비어있는 경우 null 값 리턴
            return null;
        }

        int randomNum = Random.Range(0, skillList.Count);
        return skillList[randomNum];
    }
}