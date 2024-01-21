using UnityEngine;

[CreateAssetMenu(menuName ="Game Object/Player", fileName = "PlayerData")]
public class PlayerData : ObjectData
{
    [SerializeField]
    private int _totalMP;
    /***************************************************************
     * [ 총 마력 (Total Mana Power) ]
     * 
     * 플레이어의 총 마력 수치로 각성치가 100%에 도달했을 때의 마력이다.
     * 각성치의 비율만큼 MP에 적용된다.
     ****************************************************************/

    [SerializeField] 
    private int _awakenPoint;
    /***************************************************************
     * [ 각성치 (Awaken Point) ]
     * 
     * 플레이어만의 각성 수치로 시나리오에 벗어나는 행동을 할 시 올라간다.
     * 50% 달성 시 플레이어 제어권을 잃으며, 100%를 달성할 시
     * 강제 루프를 진행한다.
     ****************************************************************/
    public int AP
    {
        get { return _awakenPoint; }
        set
        {
            if(_awakenPoint != value)
            {
                // 입력값이 음수일 경우
                if (value < 0)
                    _awakenPoint = 0;
                // 입력값이 최대치를 초과한 경우
                else if (value > _maxAwakenPoint)
                    _awakenPoint = _maxAwakenPoint;
                else
                    _awakenPoint = value;

                // ap 수치 변경에 따른 mp 값 조절
                float apRate = (float)_awakenPoint / _maxAwakenPoint;
                MP = Mathf.RoundToInt(_totalMP * apRate);
            }
        }
    }

    [SerializeField]
    private int _maxAwakenPoint;
    public int MaxAP
    {
        get { return _maxAwakenPoint; }
    }

    [SerializeField]
    private int _defensive;
    /***************************************************************
    * [ 방어력 (Defensive) ]
    * 
    * 플레이어의 방어력 수치로 받는 데미지에 영향을 끼친다.
    * 방어력 1당 1의 데미지를 줄인다.
    ****************************************************************/
    public int DEF
    {
        get { return _defensive; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _defensive = 0;
            else
                _defensive = value;
        }
    }

    [SerializeField]
    private int _stamina;
    /***************************************************************
    * [ 피로도 (Stamina) ]
    * 
    * 플레이어의 피로도 수치로 이동속도와 전투에 영향을 끼친다.
    * 피로도가 쌓일 수록 시야가 좁아지며, 일정 이상 넘어가면
    * 이동속도가 느려지기 시작한다.
    * 100%에선 일정 시간동안 시야가 가려지며, 조작이 불가능해진다.
    * 능력을 사용하면 수치가 증가하며, 사용하지 않는 동안엔 줄어든다.
    ****************************************************************/
    public int Stamina
    {
        get { return _stamina; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _stamina = 0;
            // 입력값이 최대치를 초과한 경우
            else if (value > _maxStamina)
                _stamina = _maxStamina;
            else
                _stamina = value;
        }
    }

    [SerializeField]
    private int _maxStamina;
    public int MaxStamina
    {
        get { return _maxStamina; }
    }

    /***************************************************************
    * [ 이동속도 (Speed) ]
    * 
    * 오브젝트의 이동속도로 민첩 수치에 영향을 받는다.
    * 예외적으로 대시 속도는 민첩 수치에 영향을 받지 않는다.
    ****************************************************************/
    public override int AGI
    {
        get => base.AGI;
        set
        {
            base.AGI = value;

            _runSpeed = MoveSpeed * 1.7f;
        }
    }

    // 대시 거리까지 이동하는 속도
    [SerializeField]
    private float _dashSpeed;
    public float DashSpeed 
    {
        get { return _dashSpeed; }
    }

    // 달리기 속도
    [SerializeField] 
    private float _runSpeed;
    public float RunSpeed
    {
        get
        {
            return _runSpeed;
        }
    }

    // 위치 값
    private Vector2 _pos;
    public Vector2 Position
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public void Initialization()
    {
        // hp
        MaxHP = 100;
        HP = 100;

        // strength
        STR = 5;

        // speed
        SpeedRatio = 100;
        AGI = 15;
        _dashSpeed = 17.5f;

        // mana
        MaxMana = 0;
        Mana = 0;

        // mp & ap
        _totalMP = 100;
        _maxAwakenPoint = 100;
        AP = 5;

        // defensive
        DEF = 0;

        // stamina
        _maxStamina = 100;
        Stamina = 100;
    }

    public void ReloadStat()
    {
        MaxHP = MaxHP;
        HP = HP;
        STR = STR;
        AGI = AGI;
        MaxMana = MaxMana;
        Mana = Mana;
        MP = MP;
        AP = AP;
        DEF = DEF;
        Stamina = Stamina;
    }
}