using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental;
using UnityEngine;

public class Tag
{
    private static string _npc = "NPC";
    public static string NPC { get { return _npc; } }

    private static string _monster = "Monster";
    public static string Monster { get { return _monster; } }
}

public abstract class ObjectData : ScriptableObject
{
    [Header("스테이터스")]
    [SerializeField]
    private int _healthPoint;
    /***************************************************************
     * [ 체력 (Health Point) ]
     * 
     * 오브젝트의 생명력 수치로 0이하로 떨어지면 죽는다.
     ***************************************************************/
    public virtual int HP
    {
        get { return _healthPoint; }
        set
        {
            if(_healthPoint != value)
            {
                // 입력값이 음수일 경우
                if (value < 0)
                    _healthPoint = 0;
                // 입력값이 최대치를 초과한 경우
                else if (value > _maxHealthPoint)
                    _healthPoint = _maxHealthPoint;
                else
                    _healthPoint = value;
            }
        }
    }

    [SerializeField]
    private int _maxHealthPoint;
    public int MaxHP
    {
        get { return _maxHealthPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _maxHealthPoint = 0;
            else
                _maxHealthPoint = value;
        }
    }

    [SerializeField]
    private int _strength;
    /***************************************************************
    * [ 근력 (Strength) ]
    * 
    * 오브젝트의 근력 수치로 물리 공격력에 영향을 끼친다.
    * 근력 1당 1의 데미지를 준다.
    ****************************************************************/
    public int STR
    {
        get { return _strength; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _strength = 0;
            else
                _strength = value;
        }
    }

    [SerializeField]
    private int _agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 이동속도에 영향을 끼친다.
    ****************************************************************/
    public virtual int Speed
    {
        get { return _agility; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _agility = 0;
            else
                _agility = value;
        }
    }

    [SerializeField]
    private int _regenPower;
    /***************************************************************
     * [ 재생력 (Regenerative Power) ]
     * 
     * 오브젝트의 재생 수치로 일정 주기마다 재생력만큼 체력을 회복한다.
     * 오브젝트가 사망 시 재생력이 남아있다면,
     * 재생력을 최대 체력만큼 깎고서 그만큼의 체력을 회복한다.
     * 피격당할 시 상대방의 마력만큼 재생력을 잃는다.
     ****************************************************************/
    public int RP
    {
        get { return _regenPower; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _regenPower = 0;
            // 입력값이 최대치를 초과한 경우
            else if (value > _maxRegenPower)
                _regenPower = _maxRegenPower;
            else
                _regenPower = value;
        }
    }

    [SerializeField]
    private int _maxRegenPower;
    public int MaxRP
    {
        get { return _maxRegenPower; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _maxRegenPower = 0;
            else
                _maxRegenPower = value;
        }
    }

    [SerializeField]
    private int _magicPower;
    /***************************************************************
     * [ 마력 (Magic Power) ]
     * 
     * 오브젝트의 마력 수치로 마력과 관련된 데미지와 재생력에 영향을 끼친다.
     * 마력 수치가 높아질수록 마력을 사용한 공격의 데미지가 올라간다.
     * 플레이어의 경우 각성치에 따라 마력의 초기값이 달라진다.
     ****************************************************************/
    public virtual int MP
    {
        get { return _magicPower; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _magicPower = 0;
            else
                _magicPower = value;
        }
    }
}
