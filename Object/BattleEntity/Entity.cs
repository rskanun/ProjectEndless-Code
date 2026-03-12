using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
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
    [SerializeField, InlineEditor]
    protected EntityData entityData;

    // 외부 스크립트용
    public string Name => entityData.Name;
    public Sprite Icon => entityData.Icon;
    public BattlePosition Position => entityData.Position;
    public AttackType AttackType => entityData.AttackType;
    public List<Skill> Skills => entityData.Skills;

    // 성격 AI
    private Personality _personality;
    public Personality Personality
    {
        get
        {
            if (_personality == null || _personality.type != entityData.Personality)
                _personality = Personality.OfType(entityData.Personality);

            return _personality;
        }
    }

    // 최종 스탯
    [ReadOnly, SerializeField]
    protected EntityStats _finalStats;  // 상태 효과에 따른 최종 스탯값
    public EntityStats FinalStats => _finalStats;

    // 데미지 공식
    public float AttackDmg
    {
        // 임시 데미지 공식
        get { return FinalStats.STR; }
    }

    [Header("참조 스크립트")]
    [SerializeField] protected BattleHUD hud;
    [SerializeField] protected StatusEffectManager effectManager;
    [SerializeField] protected EntitySurveyManager surveyManager;
    [SerializeField] protected EntityMotionManager motionManager;
    public BattleCameraOption cameraOption;

    // 현재 상태
    private bool _isDead;
    public bool IsDead => _isDead;
    public bool IsActing => motionManager.IsActing;
    public bool IsAttackEnd
        => !IsActing || motionManager.IsPlayAnimation(AnimParams.ReturnMotion);
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


    protected virtual void Awake()
    {

    }

    public float GetLastTurn(float originTurn)
    {
        return originTurn * (1.0f - ((FinalStats.AGI / 10) / 10.0f));
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
        GameEventManager.Instance.NotifyTurnEnded();
    }

    /***************************************************************
    * [ 일반 공격 ]
    * 
    * 단일 타겟을 대상으로 한 자원 소모 없는 일반 공격 실행 및 모션 제어
    ***************************************************************/

    public void Attack(Entity target)
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
        ActAttackAnimation(target, () => target.OnDamage(AttackDmg, FinalStats.MP, criticalChance));
    }

    private void ActAttackAnimation(Entity target, Action onHit)
    {
        // 해당 엔티티의 공격 타입에 따라 공격 모션 선택
        if (entityData.AttackType == AttackType.Melee)
            motionManager.ActMeleeAttackAnimation(target, true, true, AnimParams.AttackMotion, AnimParams.AttackTrigger, onHit);
        else
            motionManager.ActRangeAttackAnimation(target, false, true, onHit);
    }

    public void Counterattack(Entity target)
    {
        // 반격 모션 실행
        motionManager.ActCounterattackAnimation(() =>
            target.OnDamage(AttackDmg, FinalStats.MP, 1.0f));
    }

    public virtual float GetCriticalChance(Entity target)
    {
        // 크리티컬 확률 계산
        if (target.HasState(EntityState.Stagger)) return 1.0f;
        return (FinalStats.DEX - target.FinalStats.AGI) / (2.0f * FinalStats.DEX);
    }

    /***************************************************************
    * [ 스킬 ]
    * 
    * 각 스킬 사용에 따른 자원 소모 및 모션 제어
    ***************************************************************/

    public virtual void CastSkill(Skill skill, List<Entity> targets)
    {
        // SP 소모
        FinalStats.SP -= skill.CostSP;
        hud.UpdateSP(FinalStats.SP, FinalStats.MaxSP);

        // 스킬 시전
        ActSkillAnimation(targets, skill, () => skill.OnCasting(this, targets));
    }

    private void ActSkillAnimation(List<Entity> targets, Skill skill, Action onHit)
    {
        // 현재는 단일 타겟 위주의 애니메이션만 진행
        if (targets.Count > 1 || skill is not AttackSkill atkSkill)
        {
            onHit?.Invoke();
            return;
        }

        if (skill.ActionType == AttackType.Melee)
            motionManager.ActMeleeAttackAnimation(
                targets.First(),
                atkSkill.IsParryable,
                atkSkill.IsDodgeable,
                skill.SkillMotion,
                skill.SkillTrigger,
                onHit);
        else
            motionManager.ActRangeAttackAnimation(targets.First(), atkSkill.IsParryable, atkSkill.IsDodgeable, onHit);
    }

    /***************************************************************
    * [ 아이템 ]
    * 
    * 소지 중인 아이템 사용에 따른 자원 소모 및 모션 제어
    ***************************************************************/

    public virtual void UseItem(Consumable item, List<Entity> targets)
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

    public virtual void Wait()
    {
        // 시전자를 향해 싱글샷
        BattleCameraDirector.Instance.FocusSingle(gameObject);
    }

    /***************************************************************
    * [ 도주 ]
    * 
    * 해당 전투에서 벗어나는 시도 실행 및 모션 제어
    ***************************************************************/

    public virtual void Run()
    {
        // 화면 전체샷
        BattleCameraDirector.Instance.FocusFullScreen();

        // 해당 엔티티를 전투에서 영구 제외
        BattleData.Instance.RemoveEntity(this);

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
        var battleData = BattleData.Instance;

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
            motionManager.ActMotion(AnimParams.DodgeTrigger);
            return;
        }

        // 크리티컬 여부 확인
        bool isCritical = UnityEngine.Random.Range(0f, 1f) <= criticalChance;

        // 크리티컬일 경우 기존 데미지의 1.2배 + 방어력 수치 무시
        int lastDamage = isCritical ? GetLastDmg(damage * 1.2f, true) : GetLastDmg(damage, false);
        FinalStats.HP -= lastDamage;
        FinalStats.MP -= GetLastMP(attackerMP);

        // 데미지 모션
        motionManager.ActHitAnimation();

        // 데미지 표시
        DamagePopup.IndicateDamage(transform.position, lastDamage);

        // HUD 업데이트
        hud.UpdateHP(FinalStats.HP, FinalStats.MaxHP);
        hud.UpdateMP(FinalStats.MP, FinalStats.MaxMP);

        // 오브젝트 마력 고갈 처리
        if (FinalStats.MP <= 0)
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
            damage -= FinalStats.DEF;
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
        _isDead = true;

        // 시퀀스 삭제
        BattleData.Instance.Sequence.RemoveTurn(this);

        // 사망 애니메이션 실행
        motionManager.ActDeadAnimation();
    }

    public virtual void OnRevival(int hp)
    {
        var seq = BattleData.Instance.Sequence;

        // 사망 판정 철회
        _isDead = false;

        // 재생했을 때의 hp 설정(최소 1 이상의 HP로 부활)
        FinalStats.HP = (hp > 0) ? hp : 1;

        // 전투 시퀀스에 대기 상태로 행동 예약
        seq.AddTurn(new WaitAction(this, 0.0f));
    }

    public virtual void OnManaShort()
    {
        // 어떤 효과로 할 것인지 생각중...
        // 마방 0 + 기절?
    }

    public void OnTargetedAttack(Entity attacker, bool isParryable, bool isDodgeable)
    {
        if (State.HasState(EntityState.Stagger))
        {
            // 현재 흐트러진 상태면 공격 방어 불가
            return;
        }

        // 제일 리턴이 큰 패링부터 행동
        if (isParryable) OnParryAction();
        if (isDodgeable) OnDodgeAction();
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
        OnParryAction(attacker);
    }

    private async void OnParryAction(Entity attacker)
    {
        Debug.Log("Parrying");
        // 패링 모션 실행
        motionManager.ActMotion(AnimParams.ParryTrigger);

        // 모션 체크
        await UniTask.WaitWhile(() => IsActing);

        // 패링 모션이 끝까지 진행되었을 경우 통상적인 엔티티는 자신이 한 번 더 공격
        Attack(attacker);
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
        float addAGI = entityData.Stats.AGI * range;
        FinalStats.AGI += (int)Math.Round(addAGI, MidpointRounding.AwayFromZero);
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
        float subAGI = entityData.Stats.AGI * range;
        FinalStats.AGI -= (int)Math.Round(subAGI, MidpointRounding.AwayFromZero);
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
        surveyManager.SetForecastHP(FinalStats.HP, FinalStats.MaxHP, change);
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