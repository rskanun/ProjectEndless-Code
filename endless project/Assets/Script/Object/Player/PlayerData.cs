using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Player", menuName ="scriptable Object/Player")]
public class PlayerData : ObjectData
{
    private UnityEvent<Vector2> _onPlayerAngleChanged = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnPlayerAngleChanged { get { return _onPlayerAngleChanged; } }

    private const float DASH_SPEED = 0.35f; // 대쉬 거리까지 이동하는 속도
    public float DashSpeed { get { return DASH_SPEED; } }

    public override int HP
    {
        get { return base.HP; }
        set
        {
            base.HP = value;
            OnPropertyChanged("HP");
        }
    }

    [SerializeField]
    private int _awakenPoint;
    /***************************************************************
     * [ 각성치 (Awaken Point) ]
     * 
     * 플레이어만의 각성 수치로 시나리오에 벗어나는 행동을 할 시 올라간다.
     * 100%를 달성할 시 강제 루프를 진행한다.
     * 마력 수치에 따라 초기값이 달라진다.
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
                    _awakenPoint = MP;
                // 입력값이 최대치를 초과한 경우
                else if (value + MP > _maxAwakenPoint)
                    _awakenPoint = _maxAwakenPoint;
                else
                    _awakenPoint = value;

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
    private int _armorPen;
    /***************************************************************
     * [ 방어력 관통 (Armor Penetration) ]
     * 
     * 플레이어만 가지는 옵션으로 적의 방어력을 일부 무시하는 수치다.
     * 방어력 관통 1당 방어력 1을 무시한다.
     * 마력의 수치에 따라 방어력 관통 수치가 달라진다.
     ****************************************************************/
    public int ArmorPenetration
    {
        get { return _armorPen; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _armorPen = 0;
            else
                _armorPen = value;
        }
    }

    [SerializeField]
    private Vector2 _angle; // 플레이어 시선 각도
    public Vector2 Angle
    {
        get { return _angle; }
        set
        {
            if (_angle != value && value != Vector2.zero)
            {
                _angle = value;
                OnPlayerAngleChanged.Invoke(_angle);

                Debug.Log(_angle);
            }
        }
    }
}