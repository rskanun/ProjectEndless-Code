using UnityEngine;

public class ObjectTag
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
    public virtual int MaxHP
    {
        get { return _maxHealthPoint; }
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
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _strength = 0;
            else
                _strength = value;

            // 공격 데미지 업데이트
            _atkDamage = _strength;
        }
    }

    [SerializeField]
    private int _agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 이동속도에 영향을 끼친다.
    ****************************************************************/
    public virtual int AGI
    {
        get { return _agility; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _agility = 0;
            else
                _agility = value;

            // 이동속도 변경
            _moveSpeed = Mathf.RoundToInt(_agility * _speedRatio);
        }
    }

    // 민첩의 이동속도 전환율
    private float _speedRatio = 100;
    public float SpeedRatio
    {
        get { return _speedRatio; }
        protected set { _speedRatio = value; }
    }

    // 이동속도
    [SerializeField]
    private int _moveSpeed;
    public virtual int MoveSpeed
    {
        get { return _moveSpeed; }
    }

    [SerializeField]
    private int _mana;
    /***************************************************************
     * [ 마나 (Mana) ]
     * 
     * 오브젝트의 마나로 특수한 공격이나 체력 회복에 사용된다.
     * 오브젝트의 피가 닳았다면 일정 주기마다 마나를 사용해 체력을 회복한다.
     * 오브젝트가 사망 시 마나가 남아있다면,
     * 마나를 최대 체력만큼 깎고서 그만큼의 체력을 회복한다.
     * 피격당할 시 상대방의 마력만큼 마나를 잃는다.
     ****************************************************************/
    public virtual int Mana
    {
        get { return _mana; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _mana = 0;
            // 입력값이 최대치를 초과한 경우
            else if (value > _maxMana)
                _mana = _maxMana;
            else
                _mana = value;
        }
    }

    [SerializeField]
    private int _maxMana;
    public virtual int MaxMana
    {
        get { return _maxMana; }
    }

    [SerializeField]
    private int _magicPower;
    /***************************************************************
     * [ 마력 (Magic Power) ]
     * 
     * 오브젝트의 마력 수치로 마력과 관련된 데미지에 영향을 끼친다.
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

    // 일반 공격 데미지
    [SerializeField]
    private int _atkDamage;
    public virtual int AttackDamage
    {
        get { return _atkDamage; }
        protected set { _atkDamage = value; }
    }


    [ContextMenu("Initialization Data")]
    public abstract void Initialization();
}
