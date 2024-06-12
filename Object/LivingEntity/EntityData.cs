using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    [Header("오브젝트 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [Header("보유 스킬")]
    [SerializeField]
    private List<Skill> _skills;
    public List<Skill> Skills
    {
        get { return _skills; }
    }

    [Header("스테이터스")]
    [SerializeField]
    private int _healthPoint;
    /***************************************************************
     * [ 체력 (Health Point) ]
     * 
     * 오브젝트의 생명력 수치로 0이하로 떨어지면 죽는다.
     ***************************************************************/
    public int HP
    {
        get { return _healthPoint; }
    }

    [SerializeField]
    private int _strength;
    /***************************************************************
    * [ 근력 (Strength) ]
    * 
    * 오브젝트의 근력 수치로 물리 공격력에 영향을 끼친다.
    * 근력 1당 1의 데미지를 준다.
    ****************************************************************/
    public virtual int STR
    {
        get { return _strength; }
    }

    [SerializeField]
    private int _defensive;
    /***************************************************************
    * [ 방어력 (Defensive) ]
    * 
    * 오브젝트의 방어력 수치로 받는 데미지에 영향을 끼친다.
    * 방어력 1당 1의 데미지를 줄인다.
    ****************************************************************/
    public int DEF
    {
        get { return _defensive; }
    }

    [SerializeField]
    private int _agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 행동 턴 수에 영향을 끼친다.
    * 특정 행동까지 걸리는 턴 수를 민첩 수치에 따라 줄여준다.
    * 같은 턴에 행동하는 다른 오브젝트가 있다면 민첩이 높은 순서대로
    * 턴을 진행한다.
    * 조작 불가능 오브젝트의 경우 민첩 수치에 따라 공격을 회피하기도 한다.
    ****************************************************************/
    public virtual int AGI
    {
        get { return _agility; }
    }

    [SerializeField]
    private int _magicPower;
    /***************************************************************
     * [ 마력 (Magic Power) ]
     * 
     * 오브젝트의 마력 수치로 마력과 관련된 것들에 영향을 끼친다.
     * 마력 수치가 높아질수록 마력을 사용한 공격의 데미지가 올라간다.
     * 마력 수치가 0이 될 시, 기절 상태에 빠진다.
     * 플레이어의 경우 각성치에 따라 마력의 초기값이 달라진다.
     ****************************************************************/
    public virtual int MP
    {
        get { return _magicPower; }
    }

    [SerializeField]
    private int _stamina;
    /***************************************************************
    * [ 스태미나 (Stamina) ]
    * 
    * 오브젝트의 스태미나 수치로 마력 사용에 영향을 끼친다.
    * 마력을 이용한 행동을 할 때마다 일정 수치를 필요로 한다.
    ****************************************************************/
    public int Stamina
    {
        get { return _stamina; }
    }
}
