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
    [Header("이벤트")]
    [SerializeField] private GameEvent turnEndEvent;
    [SerializeField] private GameEvent deadEvent;

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
    private EntityStat _stat;
    public EntityStat Stat
    {
        protected set { _stat = value; }
        get { return _stat; }
    }

    // 현재 상태
    private bool _isDead;
    public bool IsDead
    {
        private set { _isDead = value; }
        get { return _isDead; }
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

    public abstract void OnAttack(Entity target);

    public virtual void OnCast(Skill skill, List<Entity> targets)
    {
        skill.OnCasting(this, targets);
    }

    public virtual void OnUseItem(Consumable item, List<Entity> targets)
    {
        foreach (Entity target in targets)
        {
            item.OnUse(target);
        }
    }

    public virtual void OnRun()
    {

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

        // 엔티티 사망 알림
        deadEvent.NotifyUpdate();
    }

    public abstract void OnManaShort();
}