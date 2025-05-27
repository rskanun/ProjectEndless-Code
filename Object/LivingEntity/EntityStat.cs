using UnityEngine;

[System.Serializable]
public class EntityStat
{
    const int MAX = 99;

    [SerializeField]
    private int _healthPoint;
    /***************************************************************
     * [ 체력 (Health Point) ]
     * 
     * 오브젝트의 생명력 수치로 0이하로 떨어지면 죽는다.
     ***************************************************************/
    public int HP
    {
        get => _healthPoint;
        set => _healthPoint = Mathf.Clamp(value, 0, MaxHP);
    }

    [SerializeField]
    private int _maxHP;
    public int MaxHP
    {
        get => _maxHP;
        set => _maxHP = Mathf.Clamp(value, 1, MAX);
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
        get => _strength;
        set => _strength = Mathf.Clamp(value, 0, MAX);
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
        get => _defensive;
        set => _defensive = Mathf.Clamp(value, 0, MAX);
    }

    [SerializeField]
    private int _agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 행동 턴 수와 공격 회피 및 치명타 회피에 
    * 영향을 끼친다.
    * 특정 행동까지 걸리는 턴 수를 민첩 수치에 따라 줄여준다.
    * 같은 턴에 행동하는 다른 오브젝트가 있다면 민첩이 높은 순서대로
    * 턴을 진행한다.
    * 대상의 공격 회피와 패링 및 치명타 실패 확률을 올려주기도 한다.
    * 조작 불가능 오브젝트의 경우 민첩 수치에 따라 공격을 회피하기도 한다.
    ****************************************************************/
    public int AGI
    {
        get => _agility;
        set => _agility = Mathf.Clamp(value, 0, MAX);
    }

    [SerializeField]
    private int _dexterity;
    /***************************************************************
    * [ 기교 (Dexterity) ]
    * 
    * 오브젝트의 기교 수치로 공격 패링과 치명타에 영향을 끼친다.
    * 기교 수치가 높아질수록 패링 성공 확률과 방어 확률이 높아지며,
    * 치명타를 줄 확률이 높아진다.
    * 플레이어의 경우 패링 방어 확률과 치명타 성공 여부에만 영향을 준다.
    ****************************************************************/
    public int DEX
    {
        get => _dexterity;
        set => _dexterity = Mathf.Clamp(value, 0, MAX);
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
        get => _magicPower;
        set => _magicPower = Mathf.Clamp(value, 0, MaxMP);
    }

    [SerializeField]
    private int _maxMP;
    public int MaxMP
    {
        get => _maxMP;
        set => _maxMP = Mathf.Clamp(value, 1, MAX);
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
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, MaxSP);
    }

    [SerializeField]
    private int _maxSP;
    public int MaxSP
    {
        get => _maxSP;
        set => _maxSP = Mathf.Clamp(value, 1, MAX);
    }

    [SerializeField]
    private int _sanity;
    /***************************************************************
    * [ 정신력 (Sanity) ]
    * 
    * 오브젝트의 정신력 수치로 정신상태 이상에 영향을 끼친다.
    * 정신력이 높을 수록 정신상태 이상에 걸릴 확률이 낮아진다.
    ****************************************************************/
    public int SAN
    {
        get => _sanity;
        set => _sanity = Mathf.Clamp(value, 0, MAX);
    }

    public EntityStat Clone()
    {
        EntityStat clone = new EntityStat();

        clone.MaxHP = MaxHP;
        clone.HP = HP;
        clone.STR = STR;
        clone.DEF = DEF;
        clone.AGI = AGI;
        clone.DEX = DEX;
        clone.MaxMP = MaxMP;
        clone.MP = MP;
        clone.MaxSP = MaxSP;
        clone.SP = SP;
        clone.SAN = SAN;

        return clone;
    }
}
