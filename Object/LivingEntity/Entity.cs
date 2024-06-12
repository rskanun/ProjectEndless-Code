using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // 오브젝트 스텟
    public virtual EntityData Stat
    {
        get;
    }

    public virtual int CurrentHP
    {
        get; protected set;
    }

    public virtual int CurrentMP
    {
        get; protected set;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public virtual void OnDamage(float damage, int targetMP)
    {
        // 최종 데미지 수치(임시)
        CurrentHP = Mathf.RoundToInt(damage - Stat.DEF);

        // 최종 마력 수치(임시)
        CurrentMP = CurrentMP - targetMP;

        // 오브젝트 사망 처리
        if (CurrentHP <= 0)
        {
            // HP 수치가 0 이하로 떨어질 경우 사망 처리
            OnDead();
        }

        // 오브젝트 마력 고갈 처리
        if (CurrentMP <= 0)
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