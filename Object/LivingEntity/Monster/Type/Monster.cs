using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterUI))]
public abstract class Monster : MonoBehaviour
{
    [Header("오브젝트 데이터 값")]
    [SerializeField] private MonsterData data;
    public MonsterData Data
    {
        get { return data; }
    }

    // 몬스터 스테이터스
    private int _currentHP;
    private int hp
    {
        get { return _currentHP; }
        set
        {
            if (_currentHP != value)
            {
                if (value < 0)
                    _currentHP = 0;
                else if (value > data.MaxHP)
                    _currentHP = data.MaxHP;
                else
                    _currentHP = value;

                UpdateHp();
            }
        }
    }
    private int _currentMana;
    public int mana
    {
        get { return _currentMana; }
        set
        {
            if (_currentMana != value)
            {
                if (value < 0)
                    _currentMana = 0;
                else if (value > data.MaxMana)
                    _currentMana = data.MaxMana;
                else
                    _currentMana = value;

                UpdateMana();
            }
        }
    }

    // 몬스터 성향
    private Propensity _propensity;
    public Propensity Propensity
    {
        get
        {
            if (_propensity == null) _propensity = CreatePropensity();

            return _propensity;
        }
    }

    private Propensity _curPropensity;
    public Propensity CurPropensity
    {
        get
        {
            if (_curPropensity == null) _curPropensity = Propensity;

            return _curPropensity;
        }
        set
        {
            System.Type type = value.GetType();

            // 기본 성향이나 적대적인 성향으로만 변할 수 있음
            if (type == _propensity.GetType() || type == typeof(Hostile))
            {
                _curPropensity = value;
            }
        }
    }

    // 몬스터 성격
    private Personality _personality;
    public Personality Personality
    {
        get
        {
            if (_personality == null) _personality = CreatePersonality();

            return _personality;
        }
    }

    [Header("이동 좌표 포인트")]
    [SerializeField]
    private Color lineColor; // 표시 색
    [SerializeField]
    private List<Vector2> movePoints;
    public List<Vector2> MovePoints { get { return movePoints; } }

    // 연관 스크립트
    private MonsterUI ui;
    private OrganManager organManager;
    private FSM fsm;

    private void Awake()
    {
        // Init component
        ui = gameObject.GetComponent<MonsterUI>();
        organManager = gameObject.GetComponentInChildren<OrganManager>();

        // Init FSM
        fsm = new FSM(new IdleState(this));
    }

    private void OnEnable()
    {
        InitStat();

        // idle 상태 초기화
        fsm.SetState(new IdleState(this));
    }

    private void InitStat()
    {
        hp = data.HP;
        mana = data.Mana;

        ui.InitHp(data.HP, data.MaxHP);
        ui.InitMana(data.Mana, data.MaxMana);
    }

    private void FixedUpdate()
    {
        fsm.OnAction();
    }

    private void OnDrawGizmos()
    {
        // 이동 루트
        if (movePoints.Count > 0)
        {
            Gizmos.color = lineColor;

            Vector2 prevPos = movePoints[movePoints.Count - 1];
            foreach (Vector2 pos in movePoints)
            {
                Gizmos.DrawLine(prevPos, pos);

                prevPos = pos;
            }
        }
    }

    /***************************************************************
    * [ 몬스터 초기값 설정 ]
    * 
    * 자식 클래스마다 설정할 몬스터 변수 초기값 설정
    ***************************************************************/

    protected abstract Propensity CreatePropensity();

    protected abstract Personality CreatePersonality();

    /***************************************************************
    * [ 플레이어 탐지 ]
    * 
    * 탐지 기관을 통한 플레이어 탐지
    ***************************************************************/

    public Vector3 DetectPlayer()
    {
        return organManager.DetectPlayer();
    }

    /***************************************************************
    * [ 플레이어 공격 ]
    * 
    * 몬스터의 공격 처리
    ***************************************************************/

    public abstract void OnAttack();

    /***************************************************************
    * [ 몬스터 이동 ]
    * 
    * 몬스터 이동에 따른 위치 및 애니메이션 변화 처리
    ***************************************************************/

    public void MoveTo(Vector2 movePoint)
    {
        // movePoint를 향해 이동
        float speed = data.MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, movePoint, speed);

        // 이동 방향으로 몸 회전
        RotateTo(movePoint);
    }

    public void RotateTo(Vector2 rotatePoint)
    {
        Vector2 rotateVec = (rotatePoint - (Vector2)transform.position).normalized;

        // 탐지 기관 회전
        organManager.RotateOrgans(rotateVec);
    }

    /***************************************************************
    * [ 몬스터 상태 ]
    * 
    * 공격을 받았을 때 몬스터의 상태 처리
    ***************************************************************/

    public void OnTakeDamage(int damage, int targetMP)
    {
        fsm.OnTakeDamage();

        hp -= damage;
        mana -= targetMP;

        Debug.Log(damage + " Damage!");
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }

    /***************************************************************
    * [ 변수 변화 체크 ]
    * 
    * 몬스터의 hp와 mana의 변화에 따른 UI 처리
    ***************************************************************/

    private void UpdateHp()
    {
        ui.UpdateHp(hp);

        // 체력이 0이하로 떨어지면 사망 처리
        if (hp <= 0)
        {
            OnDead();
        }
    }

    private void UpdateMana()
    {
        ui.UpdateMana(mana);
    }
}