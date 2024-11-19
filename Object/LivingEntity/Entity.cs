using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattlePosition
{
    Front,
    Back
}

public enum AttackType
{
    Melee,  // 근접 공격
    Ranged  // 원거리 공격
}

public abstract class Entity : MonoBehaviour
{
    [Header("엔티티 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private BattlePosition _position;
    public BattlePosition Position
    {
        protected set { _position = value; }
        get { return _position; }
    }

    [SerializeField]
    private AttackType _attackType;
    public AttackType AttackType
    {
        protected set { _attackType = value; }
        get { return _attackType; }
    }

    [SerializeField]
    private PersonalityType _personalityType;
    protected PersonalityType PersonalityType
    {
        set { _personalityType = value; }
    }
    private Personality _personality;
    public Personality Personality
    {
        get
        {
            if (_personality == null || _personality.type != _personalityType)
                _personality = Personality.OfType(_personalityType);

            return _personality;
        }
    }

    [Header("스킬 목록")]
    [SerializeField]
    private List<Skill> _skillList;
    public List<Skill> SkillList
    {
        protected set { _skillList = value; }
        get { return _skillList; }
    }

    [Header("스텟")]
    [SerializeField]
    private EntityStat _originStat;
    public EntityStat OriginStat
    {
        protected set { _originStat = value; }
        get { return _originStat; }
    }
    [ReadOnly, SerializeField]
    private EntityStat _lastStat;  // 상태 효과에 따른 최종 스탯값
    public EntityStat Stat
    {
        protected set { _lastStat = value; }
        get { return _lastStat; }
    }
    // 데미지 공식
    public float AttackDmg
    {
        // 임시 데미지 공식
        get { return Stat.STR; }
    }

    [Header("애니메이션")]
    [SerializeField] protected Animator animator;

    [Header("참조 스크립트")]
    [SerializeField] protected BattleHUD hud;
    [SerializeField] protected StatusEffectManager effectManager;
    [SerializeField] protected EntitySurveyManager surveyManager;

    // 현재 상태
    private bool _isDead;
    public bool IsDead
    {
        private set { _isDead = value; }
        get { return _isDead; }
    }
    private bool _isActionable;
    public bool IsActionable
    {
        private set { _isActionable = value; }
        get { return _isActionable; }
    }
    private bool _hasStun;
    public bool HasStun
    {
        private set { _hasStun = value; }
        get { return _hasStun; }
    }
    private bool isParried;

    // 전투 순서 데이터
    protected CurrentBattleData battleData { private set; get; }
    protected BattleSequence battleSeq { private set; get; }

    protected virtual void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        battleData = CurrentBattleData.Instance;
        battleSeq = battleData.Sequence;
    }

    protected void InitLastStat()
    {
        if (Stat == null)
            Stat = new EntityStat();

        Stat.MaxHP = OriginStat.MaxHP;
        Stat.HP = OriginStat.HP;
        Stat.STR = OriginStat.STR;
        Stat.DEF = OriginStat.DEF;
        Stat.AGI = OriginStat.AGI;
        Stat.MaxMP = OriginStat.MaxMP;
        Stat.MP = OriginStat.MP;
        Stat.MaxSP = OriginStat.MaxSP;
        Stat.SP = OriginStat.SP;
        Stat.SAN = OriginStat.SAN;
    }

    public float GetLastTurn(float originTurn)
    {
        return originTurn * (1.0f - ((Stat.AGI / 10) / 10.0f));
    }

    public void GatherCurTurnInfo()
    {
        // 턴 시작 시 현재 턴 정보 수집
        Personality.OnTurnStart();
    }

    /***************************************************************
    * [ 모션 ]
    * 
    * 오브젝트의 모션 실행 관리
    ***************************************************************/

    public void OnActiveMotion(string motion)
    {
        IsActionable = true;
        animator.SetTrigger(motion);
    }

    public void OnMotionEnd()
    {
        IsActionable = false;
    }

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public abstract void TakeTurn();

    public void EndTurn()
    {
        // 턴이 끝났음을 알림
        GameEventResource.Instance.EndTurnEvent.NotifyUpdate();
    }

    public void OnSelectAction(BattleAction action, int? index = null)
    {
        // 선택한 행동 예약
        battleData.Sequence.AddTurn(action);

        // 턴 종료
        EndTurn();
    }

    public virtual void OnAttack(Entity target)
    {
        // 타겟이 사망상태일 경우 다른 대상을 타겟으로 설정
        if (target != null && target.IsDead)
        {
            target = GetRetarget(target);
        }

        // 타겟이 있는 경우에만 계속해서 공격
        if (target != null)
        {
            // 치명타 여부 구하기
            float criticalChance = (Stat.DEX - target.Stat.AGI) / (2.0f * Stat.DEX);

            // 공격 모션 실행
            StartCoroutine(OnAttackAction(target, criticalChance));

            // 타겟에게 방어 유형 전달
            // 원거리는 패링 X
            target.OnTargetedAttack(this, AttackType == AttackType.Melee, true);
        }
    }

    private IEnumerator OnAttackAction(Entity target, float criticalChance)
    {
        // 공격 모션 실행
        OnActiveMotion("atk");

        // 모션 체크
        while (IsActionable)
        {
            // 공격 모션 중간 패링을 당했을 경우
            if (isParried)
            {
                isParried = false;

                // 패링 당하는 모션 실행
                OnActiveMotion("isParried");
                yield break;
            }

            yield return null;
        }

        // 공격 모션이 끝까지 진행되었을 경우 데미지
        target.OnDamage(AttackDmg, Stat.MP, Stat.DEI, criticalChance);
    }

    private Entity GetRetarget(Entity curTarget)
    {
        List<Entity> targetableList = GetTargetableList();
        List<Entity> priorityTargetList = Personality.GetPriorityTargetList(targetableList);

        // 성격에 따른 우선순위대로 다음 타겟 탐색
        foreach (Entity target in priorityTargetList)
        {
            // 만약 해당 타겟이 현재와 다른 타겟일 경우
            if (curTarget != target)
            {
                // 해당 타겟을 다음 타겟으로 설정
                return target;
            }
        }

        // 어떠한 타겟도 고를 수 없는 경우 null 리턴
        return null;
    }

    private List<Entity> GetTargetableList()
    {
        if (this is Monster) return battleData.LivingCharacters;
        else return battleData.LivingEnemies;
    }

    public void OnAssistAttack(Entity target)
    {
        // 확정 치명타인 일반 공격 실행
        StartCoroutine(OnAssistAttackAction(target));
    }

    private IEnumerator OnAssistAttackAction(Entity target)
    {
        // 공격 모션 실행
        OnActiveMotion("atk");

        // 모션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsActionable);

        // 공격 모션이 끝까지 진행되었을 경우 치명타 데미지
        target.OnDamage(AttackDmg, Stat.MP, Stat.DEI, 1.0f);
    }

    public virtual void OnCast(Skill skill, List<Entity> targets)
    {
        // SP 소모
        Stat.SP -= skill.CostSP;
        hud.UpdateSP(Stat.SP, Stat.MaxSP);

        // 스킬 시전
        skill.OnCasting(this, targets);
    }

    public virtual void OnUseItem(Consumable item, List<Entity> targets)
    {
        item.OnUse(targets);
    }

    public virtual void OnRun()
    {
        // 해당 엔티티를 전투에서 영구 제외
        battleData.RemoveEntity(this);

        // 오브젝트 삭제
        Destroy(gameObject);
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public virtual void OnDamage(float damage, int attackerMP, float attackerDEI, float criticalChance)
    {
        // 크리티컬 여부 확인
        float random = UnityEngine.Random.Range(0f, 1f);
        bool isCritical = random < criticalChance;

        // 크리티컬일 경우 기존 데미지의 1.2배 + 방어력 수치 무시
        Stat.HP -= isCritical ? GetLastDmg(damage * 1.2f, 100.0f) : GetLastDmg(damage, attackerDEI);
        Stat.MP -= GetLastMP(attackerMP);

        // 데미지 모션
        animator.SetTrigger("hit");

        // HUD 업데이트
        hud.UpdateHP(Stat.HP, Stat.MaxHP);
        hud.UpdateMP(Stat.MP, Stat.MaxMP);

        // 오브젝트 사망 처리
        if (Stat.HP <= 0)
        {
            // HP 수치가 0 이하로 떨어질 경우 사망 처리
            OnDead();
        }

        // 오브젝트 마력 고갈 처리
        if (Stat.MP <= 0)
        {
            // MP 수치가 0 이하로 떨어질 경우 마력 고갈 처리
            OnManaShort();
        }
    }

    public int GetLastDmg(float damage, float attackerDEI)
    {
        // 최종 데미지 수치(임시)
        float dmg = damage - Stat.DEF * (1.0f - attackerDEI / 100.0f);

        return Mathf.RoundToInt(dmg > 0 ? dmg : 0.0f);
    }

    public int GetLastMP(int targetMP)
    {
        // 최종 마력 데미지 수치(임시)
        return 0;
    }

    public virtual void OnDead()
    {
        // 엔티티 사망 처리
        IsDead = true;

        // 시퀀스 삭제
        battleSeq.RemoveTurns(this);

        // 사망 모션
        animator.SetTrigger("death");
    }

    public virtual void OnRevival(int hp)
    {
        if (hp <= 0)
        {
            throw new NullReferenceException("체력이 0 이하인 상태론 부활할 수 없습니다!");
        }

        // 사망 판정 철회
        IsDead = false;

        // 재생했을 때의 hp 설정
        Stat.HP = hp;

        // 전투 시퀀스에 대기 상태로 행동 예약
        battleSeq.AddTurn(new WaitAction(this, 0.0f));
    }

    public virtual void OnManaShort()
    {
        // 어떤 효과로 할 것인지 생각중...
        // 마방 0 + 기절?
    }

    public virtual void OnTargetedAttack(Entity attacker, bool isUsedParry, bool isUsedDodge)
    {
        // 플레이어가 아닌 엔티티의 경우 확률적
        // 민첩의 차이가 많이 날 수록 확률이 높아짐

        // 제일 리턴이 큰 패링부터 확률 계산
        if (isUsedParry)
        {
            // 패링이 가능하면, 패링 확률 계산하여 패링 실행 유무 결정
        }
        else if (isUsedDodge)
        {
            // 회피가 가능하면, 회피 확률 계산하여 회피 실행 유무 결정
        }
    }

    public virtual void OnParried()
    {
        // 공격이 패링 당했을 경우
        isParried = true;
    }

    public virtual void OnParrying()
    {
        // 패링에 성공했을 경우
    }

    /***************************************************************
    * [ 상태 효과 ]
    * 
    * 상태 효과 관리와 그에 따른 상태 변화 적용
    ***************************************************************/

    public void UpdateEffectTimer(float turn)
    {
        effectManager.UpdateEffectTimer(turn);
    }

    public void AddEffect(StatusEffect effect)
    {
        if (effect == null || effect.IsEmpty())
        {
            // 상태이상이 null값인 경우 상태이상 적용 X
            return;
        }

        // 효과 적용
        effectManager.AddEffect(
            effect,
            () => ApplyEffect(effect),
            () => ClearEffect(effect)
        );
    }

    public bool HasEffect(StatusEffect effect)
    {
        return effectManager.HasEffect(effect);
    }

    public float GetEffectDuration(StatusEffect effect)
    {
        return effectManager.GetDuration(effect);
    }

    private void ApplyEffect(StatusEffect effect)
    {
        foreach (StatusEffectData effectData in effect.Effects)
        {
            ApplyEffect(effectData);
        }
    }

    private void ApplyEffect(StatusEffectData effect)
    {
        switch (effect.Type)
        {
            case EffectType.Haste:
                ApplyHaste(effect.EffectRange);
                break;
        }
    }

    private void ApplyHaste(float range)
    {
        float addAGI = OriginStat.AGI * range;
        Stat.AGI += (int)Math.Round(addAGI, MidpointRounding.AwayFromZero);
    }

    private void ClearEffect(StatusEffect effect)
    {
        foreach (StatusEffectData effectData in effect.Effects)
        {
            ClearEffect(effectData);
        }
    }

    private void ClearEffect(StatusEffectData effect)
    {
        switch (effect.Type)
        {
            case EffectType.Haste:
                ClearHaste(effect.EffectRange);
                break;
        }
    }

    private void ClearHaste(float range)
    {
        float subAGI = OriginStat.AGI * range;
        Stat.AGI -= (int)Math.Round(subAGI, MidpointRounding.AwayFromZero);
    }

    /***************************************************************
    * [ 상태 관찰 ]
    * 
    * 해당 오브젝트의 상태 관찰에 따른 ui 변화
    ***************************************************************/

    public void ActiveActionIcon(ActionType type)
    {
        surveyManager.ActiveActionIcon(type);
    }

    public void HideActionIcon()
    {
        surveyManager.HideActionIcon();
    }

    public void SetForecastHP(int change)
    {
        surveyManager.SetForecastHP(Stat.HP, Stat.MaxHP, change);
    }

    public void SetActiveForecastHP(bool isActive)
    {
        surveyManager.SetActiveForecastHP(isActive);
    }

    public void SetForecastEffect(StatusEffect effect)
    {
        surveyManager.SetForecastEffect(effect);
    }

    public void ClearForecastEffect()
    {
        surveyManager.ClearForecastEffect();
    }
}