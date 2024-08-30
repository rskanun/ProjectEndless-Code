using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
    [Header("이벤트")]
    [SerializeField] private GameEvent turnEndEvent;

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
    [SerializeField]
    private EntityStat _lastStat;  // 상태 효과에 따른 최종 스탯값
    public EntityStat Stat
    {
        protected set { _lastStat = value; }
        get { return _lastStat; }
    }

    [Header("참조 스크립트")]
    [SerializeField] private StatusEffectManager effectManager;

    // 현재 상태
    private bool _isDead;
    public bool IsDead
    {
        private set { _isDead = value; }
        get { return _isDead; }
    }

    // 전투 순서 데이터
    protected BattleData battleData { private set; get; }

    protected virtual void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        battleData = BattleData.Instance;
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

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public abstract void TakeTurn();

    public void EndTurn()
    {
        // 턴이 끝났음을 알림
        turnEndEvent.NotifyUpdate();
    }

    public virtual void OnAttack(Entity target)
    {
        // 타겟이 사망상태일 경우 다른 대상을 타겟으로 설정
        if (target != null && target.IsDead)
        {
            target = OnRetarget(target);
        }

        // 타겟이 없으면 공격 종료
        if (target == null) return;

        // 타겟 공격
        target.OnDamage(Stat.STR, Stat.MP);
        Debug.Log($"{Name} Attack {target.Name}!!");
    }

    protected abstract Entity OnRetarget(Entity curTarget);

    public virtual void OnCast(Skill skill, List<Entity> targets)
    {
        // SP 소모
        Stat.SP -= skill.CostSP;

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

    public virtual void OnDamage(float damage, int targetMP)
    {
        // 최종 데미지 수치(임시)
        float lastDamage = damage - Stat.DEF;
        Stat.HP -= Mathf.RoundToInt(lastDamage > 0 ? lastDamage : 0.0f);

        // 최종 마력 수치(임시)
        Stat.MP = Stat.MP - targetMP;

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

    public virtual void OnDead()
    {
        // 엔티티 사망 처리
        IsDead = true;

        // 시퀀스 삭제
        battleData.Sequence.RemoveTurns(this);
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
        battleData.Sequence.AddTurn(new WaitAction(this, 0.0f));
    }

    public virtual void OnManaShort()
    {
        // 어떤 효과로 할 것인지 생각중...
        // 마방 0 + 기절?
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
        // 효과 적용
        effectManager.AddEffect(
            effect, 
            () => ApplyEffect(effect), 
            () => ClearEffect(effect)
        );
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
}