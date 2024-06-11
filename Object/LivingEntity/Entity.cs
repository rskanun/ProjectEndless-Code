using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("오브젝트 정보")]
    [SerializeField]
    private EntityData _data;
    public EntityData Stat
    {
        get { return _data; }
    }

    // 스테이터스
    private int _healthPoint;
    public int HP
    {
        get { return _healthPoint; }
        private set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _healthPoint = 0;
            // 입력값이 최대치를 초과한 경우
            else if (value > Stat.MaxHP)
                _healthPoint = _maxHealthPoint;
            else
                _healthPoint = value;
        }
    }
}