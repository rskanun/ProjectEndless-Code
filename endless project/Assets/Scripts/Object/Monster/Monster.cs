using System.Collections;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Revival
}
public class Monster : MonoBehaviour
{
    [Header("오브젝트 데이터 값")]
    [SerializeField] private MonsterData stat;

    // 몬스터 상태
    private MonsterState state;

    // 몬스터 스테이터스
    private int _currentHP;
    private int hp
    {
        get { return  _currentHP; }
        set
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
    private int _currentMana;
    public int mana
    {
        get { return _currentMana; }
        set
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

    // 체력 회복 주기
    private float revivalDelay = 2.0f;
    private float hpRegenCooldown = 3.0f;

    // 체력 회복 수치
    private int healingManaRate = 5;

    private MonsterUI ui;
    private Coroutine regenCoroutine;

    private void Awake()
    {
        ui = GetComponent<MonsterUI>();
    }

    private void OnEnable()
    {
        hp = stat.HP;
        mana = stat.Mana;

        ui.InitHp(stat.HP, stat.MaxHP);
        ui.InitMana(stat.Mana, stat.MaxMana);

        state = MonsterState.Idle;
    }

    /***************************************************************
    * [ 몬스터 행동 ]
    * 
    * 외부 요인이나 AI에 의한 몬스터의 행동 처리
    ***************************************************************/

    // 공격을 받았을 경우
    public void OnTakeDamage(int damage, int targetMP)
    {
        if (state == MonsterState.Idle)
        {
            hp -= damage;
            mana -= targetMP;

            Debug.Log(damage + " Damage!");
        }
    }

    // 체력이 0이 되어 죽었을 경우
    private void OnDead()
    {
        // 마나가 남아있을 경우 소생
        if (mana > 0)
        {
            StartCoroutine(RevivalHP());
        }
        // 모든 마나를 소진한 경우 사망처리
        else
        {
            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);

            Destroy(gameObject);
        }
    }

    /***************************************************************
    * [ 체력 리젠 ]
    * 
    * 일정 시간마다 잃은 체력을 마나를 사용해 회복
    ***************************************************************/

    private IEnumerator HpRegen()
    {
        WaitForSeconds wait = new WaitForSeconds(hpRegenCooldown);

        // 바로 재생하지 않도록 두는 일정 텀
        yield return wait;

        // hp가 0이 아닌 잃은 상태에서만 체력 리젠 발동
        while (stat.MaxHP > hp && hp > 0)
        {

            if (hp < stat.MaxHP)
            {
                int regenHP = GetRegenHP(hp, stat.MaxHP, mana, stat.MaxMana);
                int consumedMana = regenHP; // - 1 mana -> + 1 hp

                hp += regenHP;
                mana -= consumedMana;
            }

            yield return wait;
        }

        regenCoroutine = null;
    }

    private int GetRegenHP(int hp, int maxHP, int mana, int maxMana)
    {
        int regenHP = maxMana / healingManaRate;

        if (regenHP > maxHP - hp)
        {
            // 잃은 체력이 회복할 체력보다 적은 경우 잃은 체력만큼 회복
            regenHP = maxHP - hp;
        }

        if (regenHP > mana)
        {
            // 마력이 회복할 체력보다 적은 경우 남은 마나 값만큼 회복
            regenHP = mana;
        }
        
        return regenHP;
    }

    /***************************************************************
    * [ 소생 ]
    * 
    * 마나가 남은 상태에서 사망한 경우 소생
    ***************************************************************/

    private IEnumerator RevivalHP()
    {
        state = MonsterState.Revival;

        // 체력 회복 텀
        WaitForSeconds regenCooldown = new WaitForSeconds(0.1f);

        // 부활 애니메이션
        yield return new WaitForSeconds(revivalDelay);

        while (mana > 0 && hp < stat.MaxHP)
        {
            int regenHP = 1;

            mana -= regenHP;
            hp += regenHP;

            yield return regenCooldown;
        }

        state = MonsterState.Idle;
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
        // 현재 체력이 최대 체력보다 낮아지면 리젠 발동
        else if (regenCoroutine == null && hp < stat.MaxHP)
        {
            regenCoroutine = StartCoroutine(HpRegen());
        }
    }

    private void UpdateMana()
    {
        ui.UpdateMana(mana);
    }
}