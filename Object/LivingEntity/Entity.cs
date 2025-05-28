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

    [Header("참조 스크립트")]
    [SerializeField] protected BattleHUD hud;
    [SerializeField] protected StatusEffectManager effectManager;
    [SerializeField] protected EntitySurveyManager surveyManager;
    [SerializeField] protected EntityMotionManager motionManager;
    public BattleCameraOption cameraOption;

    // 현재 상태
    private bool _isDead;
    public bool IsDead
    {
        private set { _isDead = value; }
        get { return _isDead; }
    }
    public bool IsActing => motionManager.IsActing;
    public bool IsIdle => motionManager.IsIdle;
    private EntityStateManager _stateManager;
    protected EntityStateManager State
    {
        get
        {
            if (_stateManager == null)
                _stateManager = new EntityStateManager();

            return _stateManager;
        }
    }

    // 전투 순서 데이터
    protected BattleData battleData { private set; get; }
    protected BattleSequence battleSeq { private set; get; }

    protected virtual void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        battleData = BattleData.Instance;
        battleSeq = battleData.Sequence;
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
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public void TakeTurn()
    {
        if (battleData.IsInBattle == false)
        {
            // 전투가 끝났을 경우 행동을 하지 않고 종료
            EndTurn();
            return;
        }

        // 패링 상태 해제
        // 임시적으로 턴이 시작될 때 흐트러진 상태를 제거하도록 했으나,
        // 추후 흐트러진 상태로 만드는 스킬이 나올 수도 있으니 수정
        State.Remove(EntityState.Stagger);

        // 행동 선택
        SelectAction();
    }

    protected abstract void SelectAction();

    public void EndTurn()
    {
        // 턴이 끝났음을 알림
        GameEventResource.Instance.EndTurnEvent.NotifyUpdate();
    }

    /***************************************************************
    * [ 일반 공격 ]
    * 
    * 단일 타겟을 대상으로 한 자원 소모 없는 일반 공격 실행 및 모션 제어
    ***************************************************************/

    public virtual void OnAttack(Entity target)
    {
        // 타겟이 사망상태 혹은 선택할 수 없는 경우 다른 대상을 타겟으로 설정
        if (target == null || target.IsDead)
        {
            target = GetRetarget(target);

            // 선택할 수 있는 타겟이 없는 경우 행동 종료
            if (target == null) return;
        }

        // 치명타 여부 구하기
        float criticalChance = GetCriticalChance(target);

        // 공격 모션 실행
        OnAction(AttackAction(target, criticalChance));

        // 타겟에게 방어 유형 전달
        // 원거리는 패링 X
        bool isUsedParry = AttackType == AttackType.Melee;
        target.OnTargetedAttack(this, isUsedParry, true);
    }

    public virtual float GetCriticalChance(Entity target)
    {
        // 크리티컬 확률 계산
        // 흐트러진 상태라면 무조건 크리티컬
        if (target.State.HasState(EntityState.Stagger)) return 1.0f;
        return (Stat.DEX - target.Stat.AGI) / (2.0f * Stat.DEX);
    }

    private IEnumerator AttackAction(Entity target, float criticalChance)
    {
        // 공격 모션 실행
        ActMotion("atk");

        // 모션 체크
        while (IsActing)
        {
            // 공격 모션 중간 패링을 당했을 경우
            if (State.HasState(EntityState.Stagger))
            {
                // 패링 당하는 모션 실행
                ActMotion("isParried");
                yield break;
            }

            yield return null;
        }

        // 공격 모션이 끝까지 진행되었을 경우 데미지
        target.OnDamage(AttackDmg, Stat.MP, criticalChance);

        // 히트 모션 대기
        yield return new WaitWhile(() => target.IsActing);

        // 사망 시 사망 모션 대기
        if (target.IsDead) yield return new WaitWhile(() => target.IsActing);
    }

    /***************************************************************
    * [ 스킬 ]
    * 
    * 각 스킬 사용에 따른 자원 소모 및 모션 제어
    ***************************************************************/

    public virtual void OnCast(Skill skill, List<Entity> targets)
    {
        // SP 소모
        Stat.SP -= skill.CostSP;
        hud.UpdateSP(Stat.SP, Stat.MaxSP);

        // 스킬 시전
        skill.OnCasting(this, targets);
    }

    /***************************************************************
    * [ 아이템 ]
    * 
    * 소지 중인 아이템 사용에 따른 자원 소모 및 모션 제어
    ***************************************************************/

    public virtual void OnUseItem(Consumable item, List<Entity> targets)
    {
        // 이후 

        // 아이템 사용
        item.OnUse(targets);
    }

    /***************************************************************
    * [ 대기 ]
    * 
    * 아무런 행동도 하지 않고서 일정 턴 진행
    ***************************************************************/

    public virtual void OnWait()
    {
        // 시전자를 향해 싱글샷
        BattleCameraDirector.Instance.FocusSingle(gameObject);
    }

    /***************************************************************
    * [ 도주 ]
    * 
    * 해당 전투에서 벗어나는 시도 실행 및 모션 제어
    ***************************************************************/

    public virtual void OnRun()
    {
        // 화면 전체샷
        BattleCameraDirector.Instance.FocusFullScreen();

        // 해당 엔티티를 전투에서 영구 제외
        battleData.RemoveEntity(this);

        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    /***************************************************************
    * [ 타겟 탐색 ]
    * 
    * 만약 지정한 타겟에게 모종의 이유로 행동을 실행할 수 없는 경우
    * 행동과 성격에 따른 새로운 타겟을 탐색하는 과정 설정
    ***************************************************************/

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
        return (this is Monster) ? battleData.LivingCharacters
            : battleData.LivingEnemies;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public virtual void OnDamage(float damage, int attackerMP, float criticalChance)
    {
        if (State.HasState(EntityState.Dodge))
        {
            // 회피에 성공했다면, 데미지 무시
            State.Remove(EntityState.Dodge);

            // 회피 모션 실행
            ActMotion("dodge");
            return;
        }

        // 크리티컬 여부 확인
        bool isCritical = UnityEngine.Random.Range(0f, 1f) <= criticalChance;

        // 크리티컬일 경우 기존 데미지의 1.2배 + 방어력 수치 무시
        Stat.HP -= isCritical ? GetLastDmg(damage * 1.2f, true) : GetLastDmg(damage, false);
        Stat.MP -= GetLastMP(attackerMP);

        // 데미지 모션
        ActMotion("hit");

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

    public int GetLastDmg(float damage, bool isTrueDmg)
    {
        // 최종 데미지 수치
        if (isTrueDmg == false)
        {
            // 고정 데미지가 아닐 경우 원래 데미지에 방어력 수치만큼 경감
            damage -= Stat.DEF;
        }

        return Mathf.RoundToInt(damage > 0 ? damage : 0.0f);
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
        battleSeq.RemoveTurn(this);

        // 사망 모션
        ActMotion("death");
    }

    public virtual void OnRevival(int hp)
    {
        // 사망 판정 철회
        IsDead = false;

        // 재생했을 때의 hp 설정(최소 1 이상의 HP로 부활)
        Stat.HP = (hp > 0) ? hp : 1;

        // 전투 시퀀스에 대기 상태로 행동 예약
        battleSeq.AddTurn(new WaitAction(this, 0.0f));
    }

    public virtual void OnManaShort()
    {
        // 어떤 효과로 할 것인지 생각중...
        // 마방 0 + 기절?
    }

    public void OnTargetedAttack(Entity attacker, bool isUsedParry, bool isUsedDodge)
    {
        if (State.HasState(EntityState.Stagger))
        {
            // 현재 흐트러진 상태면 공격 방어 불가
            return;
        }

        // 제일 리턴이 큰 패링부터 행동
        if (isUsedParry) OnParryAction();
        if (isUsedDodge) OnDodgeAction();
    }

    protected virtual void OnParryAction()
    {
        // 플레이어가 아닌 엔티티의 경우 확률적
        // 패링이 가능하면, 패링 확률 계산하여 패링 실행 유무 결정
    }

    protected virtual void OnDodgeAction()
    {
        // 플레이어가 아닌 엔티티의 경우 확률적
        // 회피가 가능하면, 회피 확률 계산하여 회피 실행 유무 결정
    }

    public virtual void OnParried()
    {
        // 공격이 패링 당했을 경우
        // 해당 엔티티에게 흐트러짐 상태 추가
        State.Add(EntityState.Stagger);
    }

    public virtual void OnParrying(Entity attacker)
    {
        // 패링에 성공했을 경우 패링 모션 실행
        StartCoroutine(OnParryAction(attacker));
    }

    private IEnumerator OnParryAction(Entity attacker)
    {
        Debug.Log("Parrying");
        // 패링 모션 실행
        ActMotion("parrying");

        // 모션 체크
        yield return new WaitUntil(() => IsActing == false);

        // 패링 모션이 끝까지 진행되었을 경우 통상적인 엔티티는 자신이 한 번 더 공격
        OnAttack(attacker);
    }

    public virtual void OnDodge()
    {
        State.Add(EntityState.Dodge);
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

    public bool HasState(EntityState state)
    {
        return State.HasState(state);
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