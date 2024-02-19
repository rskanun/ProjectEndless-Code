using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(MonsterUI))]
[RequireComponent (typeof(AIMonsterControlled))]
public class Monster : MonoBehaviour
{
    [Header("오브젝트 데이터 값")]
    [SerializeField] private MonsterData stat;
    public MonsterData Stat
    {
        get { return stat; }
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
                else if (value > stat.MaxHP)
                    _currentHP = stat.MaxHP;
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
                else if (value > stat.MaxMana)
                    _currentMana = stat.MaxMana;
                else
                    _currentMana = value;

                UpdateMana();
            }
        }
    }

    // 현재 몬스터 상태
    private IMonsterState currentState;

    // 연관 스크립트
    private MonsterUI ui;
    private AIMonsterControlled ai;

    private void Awake()
    {
        ui = gameObject.GetComponent<MonsterUI>();
        ai = gameObject.GetComponent<AIMonsterControlled>();
    }

    private void OnEnable()
    {
        initStat();

        // idle 상태 초기화
        SetState(IdleState.Instance);
    }

    private void initStat()
    {
        hp = stat.HP;
        mana = stat.Mana;

        ui.InitHp(stat.HP, stat.MaxHP);
        ui.InitMana(stat.Mana, stat.MaxMana);
    }

    private void FixedUpdate()
    {
        currentState.OnAction(ai);
    }

    public void SetState(IMonsterState state)
    {
        currentState = state;

        currentState.OnEnterState(ai);
    }

    /***************************************************************
    * [ 몬스터 상태 ]
    * 
    * 공격을 받았을 때 몬스터의 상태 처리
    ***************************************************************/

    public void OnTakeDamage(int damage, int targetMP)
    {
        currentState.OnTakeDamage(ai);

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