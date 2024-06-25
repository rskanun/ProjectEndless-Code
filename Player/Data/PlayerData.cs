using UnityEngine;

[CreateAssetMenu(menuName ="Game Object/Player", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("플레이어 정보")]
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
                else if (value > MaxAP)
                    _awakenPoint = MaxAP;
                else
                    _awakenPoint = value;
            }
        }
    }

    private int _maxAP = 100;
    public int MaxAP
    {
        get { return _maxAP; }
    }

    // 위치 값
    [SerializeField]
    private Vector2 _pos;
    public Vector2 Position
    {
        get { return _pos; }
        set { _pos = value; }
    }
}