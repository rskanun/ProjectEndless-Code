using System.Collections.Generic;
using UnityEngine;

public enum BattlePosition
{
    Front,
    Back
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
        get { return _position; }
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

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public virtual void OnDamage(float damage, int targetMP)
    {
        // 최종 데미지 수치(임시)
        Stat.HP = Mathf.RoundToInt(damage - Stat.DEF);

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

    public virtual void OnAttack(Entity target)
    {

    }

    public virtual void OnDead()
    {

    }

    public virtual void OnManaShort()
    {

    }
}