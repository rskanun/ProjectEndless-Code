using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class NormalMonster : Monster
{
    [Header("테스트용 타겟 선정 디스플레이")]
    public TestAiDisplay testDisplay;
    /***************************************************************
    * [ 행동 패턴 ]
    * 
    * 일반 몬스터의 공격 패턴 AI
    ***************************************************************/

    protected override void SelectAction()
    {
        Invoke(nameof(OnThink), 2.0f);
    }

    private void OnThink()
    {
        List<Entity> targetableChr = battleData.LivingCharacters;
        if (targetableChr.Count == 1)
        {
            // 플레이어 진형의 캐릭터가 한 명 남았을 경우의 선택
            SelectAction(targetableChr[0]);
        }
        else if (targetableChr.Count > 1)
        {
            // 플레이어 진형의 캐릭터가 여러 명 남았을 경우의 선택
            SelectAction(targetableChr);
        }
    }

    private void SelectAction(Entity target)
    {
        testDisplay.SetPriorityTarget(target);

        // 플레이어의 다음 턴에 맞춰 해당 턴 안에 사용 가능한 스킬 찾기
        List<Skill> usableSkills = GetUsableSkills(SkillList, target);

        if (AnyUsableSkills(usableSkills))
        {
            // 플레이어의 다음 턴 안에 사용 가능한 스킬 찾기
            List<Skill> inTurnSkills = GetUsableSkillsInTurn(usableSkills, GetPlayerNextTurn());

            // 턴 안에 사용 가능한 스킬이 있을 경우 가장 턴 수가 긴 스킬 사용
            Skill castSkill = GetMaxTurnSkill(inTurnSkills);
            if (castSkill == null)
            {
                // 턴 안에 사용 가능한 스킬이 없는 경우 가장 짧은 턴의 스킬 사용
                castSkill = GetMinTurnSkill(usableSkills);
            }

            // 선택된 스킬 사용
            SelectSkill(castSkill, target);
            return;
        }

        // 어떠한 스킬도 사용할 수 없으면 일반 공격
        SelectAttack(target);
    }

    private void SelectAction(List<Entity> targetList)
    {
        // 성격(우선 순위)에 따른 타겟 정렬 리스트
        List<Entity> sortList = Personality.GetPriorityTargetList(targetList);

        testDisplay.SetPriorityTargets(sortList);
        foreach (Entity target in sortList)
        {
            int remainHP = target.Stat.HP - target.GetLastDmg(AttackDmg, false);
            if (IsAttackTargetable(target) && remainHP <= 0)
            {
                // 대상이 일반 공격으로 해치울 수 있는 피일 경우 일반 공격
                SelectAttack(target);
                return;
            }

            // 사용 가능한 스킬 탐지
            List<Skill> usableSkills = GetUsableSkills(SkillList, target);
            if (AnyUsableSkills(usableSkills))
            {
                // 타겟의 남은 턴 수 확인
                BattleAction targetAction = battleSeq.GetEntityAction(target);
                float remainTurn = targetAction.remainTurn;

                // 턴 수 내에 사용 가능한 스킬 사용
                List<Skill> inTurnSkillList = GetUsableSkillsInTurn(usableSkills, remainTurn);

                if (AnyUsableSkills(inTurnSkillList))
                {
                    // 대상을 마무리 지을 스킬 찾기
                    Skill castSkill = GetFinishingSkill(inTurnSkillList, target);

                    if (castSkill == null)
                    {
                        // 대상을 마무리 지을 스킬이 없으면 가장 턴 수가 긴 스킬 찾기
                        castSkill = GetMaxTurnSkill(inTurnSkillList);
                    }

                    // 찾아낸 스킬 사용
                    SelectSkill(castSkill, target);
                    return;
                }
                else
                {
                    // 턴 수 내에 사용할 스킬이 없는 경우
                    // SP 내에 사용 가능한 스킬 중 대상을 마무리 지을 스킬 찾기
                    Skill castSkill = GetFinishingSkill(usableSkills, target);

                    if (castSkill == null)
                    {
                        // 대상을 마무리 지을 스킬이 없으면 가장 턴 수가 짧은 스킬 찾기
                        castSkill = GetMinTurnSkill(usableSkills);
                    }

                    // 찾아낸 스킬 사용
                    SelectSkill(castSkill, target);
                    return;
                }
            }

            // 사용 가능한 스킬이 없는 경우
            if (IsAttackTargetable(target))
            {
                // 일반 공격이 가능한 경우엔 일반 공격 실행
                SelectAttack(target);
                return;
            }
        }
    }

    private bool AnyUsableSkills(List<Skill> skillList)
    {
        return skillList != null && skillList.Any();
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

    private bool IsAttackTargetable(Entity target)
    {
        // 해당 타겟에게 일반 공격이 가능할 조건
        return (target.Position == BattlePosition.Front) // 공격 대상이 전방이거나
            || (AttackType == AttackType.Ranged); // 공격을 가하는 대상이 원거리 공격을 할 경우
    }

    private bool IsSkillTargetable(Entity target, Skill castSkill)
    {
        return castSkill.TargetType switch
        {
            TargetType.FrontEnemy => target is Monster && target.Position == BattlePosition.Front,
            TargetType.Enemy or TargetType.EnemyParty => target is Monster,
            TargetType.FrontMember => target is Character && target.Position == BattlePosition.Front,
            TargetType.Member or TargetType.PlayerParty => target is Character,
            TargetType.One or TargetType.Every => true,
            _ => false
        };
    }

    private List<Skill> GetUsableSkills(List<Skill> skillList, Entity target)
    {
        return FindSkills(skillList, (skill)
            => IsSkillTargetable(target, skill) && skill.CostSP <= Stat.SP);
    }

    private List<Skill> GetUsableSkillsInTurn(List<Skill> skillList, float remainTurn)
    {
        return FindSkills(skillList, (skill) => skill.CostTurn <= remainTurn);
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
            int lastDmg = target.GetLastDmg(skillDmg, false);
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


    // 테스트용 디스플레이 함수
    protected override void SelectAttack(Entity target, int? index = null)
    {
        testDisplay.SetSelectTarget(target);
        base.SelectAttack(target, index);
    }

    protected override void SelectSkill(Skill skill, List<Entity> targets, int? index = null)
    {
        testDisplay.SetSelectTarget(targets[0]);
        base.SelectSkill(skill, targets, index);
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public override void OnDead()
    {
        base.OnDead();

        // 일반 몬스터의 경우 사망 시 페이드 아웃 -> 개체 파괴
        StartCoroutine(OnDeadAnimation());
    }

    public IEnumerator OnDeadAnimation()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // 사망 모션 실행까지 대기
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

        // 사망 모션 시작과 동시에 페이드 아웃
        DOTween.Sequence()
            .Append(sprite.DOFade(0.0f, 1.5f))
            .OnComplete(() => Destroy(gameObject));
    }
}