using System.Collections.Generic;
using System.Linq;

public class NormalMonster : Monster
{
    /***************************************************************
    * [ 행동 패턴 ]
    * 
    * 일반 몬스터의 공격 패턴 AI
    ***************************************************************/

    protected override void SelectAction()
    {
        List<Entity> targetableChr = battleData.LivingCharacter;
        if (targetableChr.Count == 1)
        {
            // 플레이어 진형의 캐릭터가 한 명 남았을 경우의 선택
            SelectSingleCharacterAction(targetableChr[0]);
        }
        else if (targetableChr.Count > 1)
        {
            // 플레이어 진형의 캐릭터가 여러 명 남았을 경우의 선택
            SelectMultiCharacterAction();
        }
    }

    private void SelectSingleCharacterAction(Entity target)
    {
        // 플레이어의 다음 턴에 맞춰 해당 턴 안에 사용 가능한 스킬 찾기
        List<Skill> usableSkills = FindSkills(SkillList, skill => (skill.CostSP <= Stat.SP));

        if (usableSkills != null && usableSkills.Count > 0)
        {
            // 플레이어의 다음 턴 안에 사용 가능한 스킬 찾기
            float remainTurn = GetPlayerNextTurn();
            List<Skill> inTurnSkills = FindSkills(usableSkills, skill => (skill.CostTurn <= remainTurn));

            if (inTurnSkills != null && inTurnSkills.Count > 0)
            {
                // 턴 안에 사용 가능한 스킬이 있을 경우 가장 턴 수가 긴 스킬 사용
                Skill castSkill = GetMaxTurnSkill(inTurnSkills);

                SelectSkill(castSkill, target);
            }
            else
            {
                // 턴 안에 사용 가능한 스킬이 없는 경우 가장 짧은 턴의 스킬 사용
                Skill castSkill = GetMinTurnSkill(usableSkills);

                SelectSkill(castSkill, target);
            }
        }
        else
        {
            // 어떠한 스킬도 사용할 수 없으면 일반 공격
            SelectAttack(target);
        }
    }

    private void SelectMultiCharacterAction()
    {
        // 성격(우선 순위)에 따른 타겟 선택
        Entity target = Personality.SelectTarget();

        int remainHP = target.Stat.HP - target.GetLastDmg(AttackDmg);
        if (remainHP <= 0)
        {
            // 대상이 일반 공격으로 해치울 수 있는 피일 경우 일반 공격
            SelectAttack(target);
            return;
        }

        // 사용 가능한 스킬 탐지
        List<Skill> usableSkills = FindSkills(SkillList, skill => (skill.CostSP <= Stat.SP));
        if (usableSkills != null && usableSkills.Count > 0)
        {
            Skill castSkill = null;

            // 타겟의 남은 턴 수 확인
            BattleAction targetAction = battleSeq.GetTurnAction(target);
            float remainTurn = targetAction.remainTurn;

            // 턴 수 내에 사용 가능한 스킬 사용
            List<Skill> inTurnSkillList = FindSkills(usableSkills, skill => (skill.CostTurn <= remainTurn));
            if (inTurnSkillList != null && inTurnSkillList.Count > 0)
            {
                // 대상을 마무리 지을 스킬 찾기
                castSkill = GetFinishingSkill(inTurnSkillList, target);

                if (castSkill == null)
                {
                    // 대상을 마무리 지을 스킬이 없으면 가장 턴 수가 긴 스킬 찾기
                    castSkill = GetMaxTurnSkill(inTurnSkillList);
                }

                // 찾아낸 스킬 사용
                SelectSkill(castSkill, target);
                return;
            }

            // SP 내에 사용 가능한 스킬 중 대상을 마무리 지을 스킬 찾기
            castSkill = GetFinishingSkill(usableSkills, target);

            if (castSkill == null)
            {
                // 대상을 마무리 지을 스킬이 없으면 가장 턴 수가 짧은 스킬 찾기
                castSkill = GetMinTurnSkill(usableSkills);
            }

            // 찾아낸 스킬 사용
            SelectSkill(castSkill, target);
            return;
        }

        // 사용 가능한 스킬이 없는 경우 일반 공격
        SelectAttack(target);
    }

    private float GetPlayerNextTurn()
    {
        foreach (BattleAction action in battleSeq.Sequence)
        {
            if (action.actor is Character)
            {
                // 가장 가까운 캐릭터의 턴 리턴
                return action.remainTurn;
            }
        }

        // 플레이어가 없을 경우 더미 데이터 리턴
        return -1;
    }

    private List<Skill> FindSkills(List<Skill> skillList, System.Func<Skill, bool> filter)
    {
        return skillList.Where(filter).ToList();
    }

    private Skill GetMaxTurnSkill(List<Skill> skillList)
    {
        if (skillList == null || skillList.Count <= 0)
        {
            // 스킬 리스트가 비어있을 경우 null 값 리턴
            return null;
        }

        // 매개변수로 받은 리스트 중 가장 높은 턴을 소모하는 스킬 리턴
        Skill maxTurnSkill = skillList[0];
        foreach (Skill skill in skillList)
        {
            if (skill.CostTurn > maxTurnSkill.CostTurn)
            {
                maxTurnSkill = skill;
            }
        }

        return maxTurnSkill;
    }

    private Skill GetMinTurnSkill(List<Skill> skillList)
    {
        if (skillList == null || skillList.Count <= 0)
        {
            // 스킬 리스트가 비어있을 경우 null 값 리턴
            return null;
        }

        // 매개변수로 받은 리스트 중 가장 낮은 턴을 소모하는 스킬 리턴
        Skill minTurnSkill = skillList[0];
        foreach (Skill skill in skillList)
        {
            if (skill.CostTurn < minTurnSkill.CostTurn)
            {
                minTurnSkill = skill;
            }
        }

        return minTurnSkill;
    }

    private Skill GetFinishingSkill(List<Skill> skillList, Entity target)
    {
        if (skillList == null || skillList.Count <= 0)
        {
            // 스킬 리스트가 비어있을 경우 null 값 리턴
            return null;
        }

        // 매개변수로 받은 리스트 중 타겟을 죽일 수 있는 스킬 리턴
        Skill finishingSkill = null;
        foreach (Skill skill in skillList)
        {
            AttackSkill attackSkill = skill as AttackSkill;

            if (attackSkill == null)
            {
                // 공격 스킬이 아닌 경우 스킵
                continue;
            }

            // 해당 스킬이 대상을 해치울 수 있는 스킬인지 체크
            float skillDmg = attackSkill.GetSkillDmg(this);
            int lastDmg = target.GetLastDmg(skillDmg);
            if (target.Stat.HP - lastDmg <= 0)
            {
                // 대상을 해치울 스킬이 복수일 경우 더 효율적인 스킬 사용
                if (finishingSkill == null || skill.CostTurn < finishingSkill.CostTurn)
                {
                    // 리턴할 스킬이 현재 없거나, 더 빨리 사용가능할 경우 해당 스킬을 후보로 선정
                    finishingSkill = skill;
                }
            }
        }

        return finishingSkill;
    }
}