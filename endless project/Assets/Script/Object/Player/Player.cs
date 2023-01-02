using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName ="scriptable Object/Player")]
public class Player : ObjectData
{
    private const float DASH_CONSTANT = 1f; // 대쉬 이동거리 계산에 쓰일 상수값
    public float DashConstant { get { return DASH_CONSTANT; } }

    private const float DASH_SPEED = 0.35f; // 대쉬 거리까지 이동하는 속도
    public float DashSpeed { get { return DASH_SPEED; } }

    [SerializeField]
    private int awakenPoint;
    /***************************************************************
     * [ 각성치 (Awaken Point) ]
     * 
     * 플레이어만의 각성 수치로 시나리오에 벗어나는 행동을 하거나,
     * 공격을 받을 시 그 수치가 올라간다.
     * 100%를 달성할 시 강제 루프를 진행한다.
     * hp 대신 쓰이며, 마력 수치에 따라 초기값이 달라진다.
     * 시간의 경과에 따라 각성치가 초기값까지 떨어진다.
     ****************************************************************/
    public int ap
    {
        get { return awakenPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                awakenPoint = manaPoint;
            // 입력값이 최대치를 초과한 경우
            else if (value + manaPoint > maxAwakenPoint)
                awakenPoint = maxAwakenPoint;
            else
                awakenPoint = value;
        }
    }

    [SerializeField]
    private int maxAwakenPoint;
    public int maxAp
    {
        get { return maxAwakenPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                maxAwakenPoint = 0;
            else
                maxAwakenPoint = value;
        }
    }

    [SerializeField]
    private int staminaPoint;
    /***************************************************************
     * [ 기력 (Stamina Point) ]
     * 
     * 플레이어의 기력 수치로 능력 사용에 영향을 끼친다.
     * 일정 기력 수치를 사용하여 능력을 사용할 수 있다.
     * 기력을 모두 소모하면 일정시간 동안 움직일 수 없다.
     ****************************************************************/
    public int sp
    {
        get { return staminaPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                staminaPoint = 0;
            // 입력값이 최대치를 초과한 경우
            else if (value > maxStaminaPoint)
                staminaPoint = maxStaminaPoint;
            else
                staminaPoint = value;
        }
    }

    [SerializeField]
    private int maxStaminaPoint;
    public int maxSP
    {
        get { return maxStaminaPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                maxStaminaPoint = 0;
            else
                maxStaminaPoint = value;
        }
    }

    [SerializeField]
    private int manaPoint;
    /***************************************************************
     * [ 마력 (Mana Point) ]
     * 
     * 플레이어의 마력 사용 수치로 능력에 영향을 끼친다.
     * 마력이 증가할 수록, 능력의 데미지 또한 증가한다.
     * 마력의 수치에 따라 각성치 초기값이 달라진다.
     ****************************************************************/
    public int mp
    {
        get { return manaPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                manaPoint = 0;
            else
                manaPoint = value;
        }
    }
}