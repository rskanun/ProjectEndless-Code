using UnityEngine;

[System.Serializable]
public class EntityStat
{
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
        set
        {
            if (value > MaxHP)
                _healthPoint = value;
            else if (value < 0)
                _healthPoint = 0;
            else
                _healthPoint = value;
        }
    }

    [SerializeField]
    private int _maxHP;
    public int MaxHP
    {
        get { return _maxHP; }
        set
        {
            if (value < 0)
                _maxHP = 1;
            else
                _maxHP = value;
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
            if (value < 0)
                _strength = 0;
            else
                _strength = value;
        }
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
        set
        {
            if (value < 0)
                _defensive = 0;
            else
                _defensive = value;
        }
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
    public int AGI
    {
        get { return _agility; }
        set
        {
            if (value < 0)
                _agility = 0;
            else
                _agility = value;
        }
    }

    [SerializeField]
    private int _defensiveIgnore;
    /***************************************************************
    * [ 방어력 무시 (Defensive Ignore) ]
    * 
    * 오브젝트의 방어력 무시 수치로 대상의 방어력에 영향을 끼친다.
    * 최종 데미지 계산 시, 대상의 방어력을 방어력 무시의 %만큼 깍아내린다.
    ****************************************************************/
    public int DEI
    {
        get { return _defensiveIgnore; }
        set
        {
            if (value < 0)
                _defensiveIgnore = 0;
            else
                _defensiveIgnore = value;
        }
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
    public int MP
    {
        get { return _magicPower; }
        set
        {
            if (value > MaxMP)
                _magicPower = MaxMP;
            else if (value < 0)
                _magicPower = 0;
            else
                _magicPower = value;
        }
    }

    [SerializeField]
    private int _maxMP;
    public int MaxMP
    {
        get { return _maxMP; }
        set
        {
            if (value < 0)
                _maxMP = 1;
            else
                _maxMP = value;
        }
    }

    [SerializeField]
    private int _stamina;
    /***************************************************************
    * [ 스태미나 (Stamina Point) ]
    * 
    * 오브젝트의 스태미나 수치로 마력 사용에 영향을 끼친다.
    * 마력을 이용한 행동을 할 때마다 일정 수치를 필요로 한다.
    ****************************************************************/
    public int SP
    {
        get { return _stamina; }
        set
        {
            if (value > MaxSP)
                _stamina = MaxSP;
            else if (value < 0)
                _stamina = 0;
            else
                _stamina = value;
        }
    }

    [SerializeField]
    private int _maxSP;
    public int MaxSP
    {
        get { return _maxSP; }
        set
        {
            if (value < 0)
                _maxSP = 1;
            else
                _maxSP = value;
        }
    }

    [SerializeField]
    private int _sanity;
    /***************************************************************
    * [ 정신력 (Sanity) ]
    * 
    * 오브젝트의 정신상태 이상에 영향을 끼친다.
    * 정신력이 높을 수록 정신상태 이상에 걸릴 확률이 낮아진다.
    ****************************************************************/
    private readonly int MaxSAN = 100;
    public int SAN
    {
        get { return _sanity; }
        set
        {
            if (value > MaxSAN)
                _sanity = MaxSAN;
            else if (value < 0)
                _sanity = 0;
            else
                _sanity = value;
        }
    }
}
