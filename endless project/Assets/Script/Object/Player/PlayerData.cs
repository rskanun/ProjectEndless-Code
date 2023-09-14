using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Player", menuName ="scriptable Object/Player")]
public class PlayerData : ObjectData, INotifyPropertyChanged
{
    [SerializeField] private int _awakenPoint;
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
                MP = _totalMP * (_awakenPoint / _maxAwakenPoint);

                OnPropertyChanged("AP");
            }
        }
    }

    [SerializeField]
    private int _maxAwakenPoint;
    public int MaxAP
    {
        get { return _maxAwakenPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _maxAwakenPoint = 0;
            else
                _maxAwakenPoint = value;
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
            // 입력값이 음수일 경우
            if (value < 0)
                _defensive = 0;
            else
                _defensive = value;
        }
    }

    // 플레이어 총 마력 수치
    [SerializeField]
    private int _totalMP;

    // 대쉬 거리까지 이동하는 속도
    [SerializeField]
    private float _dashSpeed = 0.35f;
    public float DashSpeed 
    {
        get { return _dashSpeed; }
    }

    // 달리기 속도
    [SerializeField] 
    private float _runSpeed; // speed * 1.25
    public float RunSpeed
    {
        get
        {
            return _runSpeed;
        }
    }    

    // 민첩 수치 변경에 따른 달리기 속도 변화
    public override int Speed
    {
        get { return base.Speed; }
        set
        {
            base.Speed = value;

            // 달리기 속도 조정
            _runSpeed = Speed * 1.25f;
        }
    }

    // AP에 의해서만 MP값이 수정되도록 변경
    public override int MP
    {
        get { return base.MP; }
    }

    // HP 애니메이션 추가용 override
    public override int HP
    {
        get { return base.HP; }
        set
        {
            base.HP = value;
            OnPropertyChanged("HP");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Space]
    [Header("플레이어블 캐릭터 상태")]

    // 플레이어 시선 각도
    [SerializeField]
    private Vector2 _angle; 
    public Vector2 Angle
    {
        get { return _angle; }
        set
        {
            if (_angle != value && value != Vector2.zero)
            {
                _angle = value;
                OnPlayerAngleChanged.Invoke(_angle);
            }
        }
    }

    private UnityEvent<Vector2> _onPlayerAngleChanged = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnPlayerAngleChanged 
    {
        get { return _onPlayerAngleChanged; }
    }

    // 플레이어와 근접한 NPC
    private NPC _npc;
    public NPC Npc
    {
        get { return _npc; }
        set { _npc = value; }
    }
}