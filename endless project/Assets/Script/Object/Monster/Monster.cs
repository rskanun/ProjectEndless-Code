using System.Collections;
using System.ComponentModel;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("이벤")]

    [Header("오브젝트 데이터 값")]
    [SerializeField] private MonsterData data;

    // 체력 회복 주기
    private float hpRegenCooldown = 3.0f;

    // 체력 리젠 코루틴
    private Coroutine regenCoroutine = null;

    

    /***************************************************************
    * [ 체력 리젠 ]
    * 
    * 일정 시간마다 잃은 체력을 마나를 사용해 회복
    ***************************************************************/

    private IEnumerator hpRege()
    {
        WaitForSeconds wait = new WaitForSeconds(hpRegenCooldown);

        // 바로 재생하지 않도록 두는 일정 텀
        yield return wait;

        // hp가 0이 아닌 잃은 상태에서만 체력 리젠 발동
        while (data.MaxHP > data.HP && data.HP > 0)
        {

            if (data.HP < data.MaxHP)
            {
                int regenHP = getRegenHP(data.HP, data.MaxHP, data.Mana, data.MaxMana);
                int consumedMana = regenHP; // - 1 mana -> + 1 hp

                data.HP += regenHP;
                data.Mana -= consumedMana;
            }

            yield return wait;
        }

        regenCoroutine = null;
    }

    private int getRegenHP(int hp, int maxHP, int mana, int maxMana)
    {
        int regenHP = maxMana / 5;

        // 마력이 회복할 체력보다 적은 경우 남은 마나 값만큼 회복
        if (regenHP > mana) regenHP = mana;
        // 잃은 체력이 회복할 체력보다 적은 경우 잃은 체력만큼 회복
        if (regenHP > maxHP - hp) regenHP = maxHP - hp;

        return regenHP;
    }

    /***************************************************************
    * [ 몬스터 행동 ]
    * 
    * 외부 요인이나 AI에 의한 몬스터의 행동 처리
    ***************************************************************/

    // 공격을 받았을 경우
    public void OnTakeDamage(int damage, int targetMP)
    {
        data.HP -= damage;
        data.Mana -= targetMP;

        Debug.Log(damage + " Damage!");
    }

    // 체력이 0이 되어 죽었을 경우
    private void OnDead()
    {
        // 마나가 남아있을 경우 소생
        if (data.Mana > 0)
        {
            StartCoroutine(revivalHP());
        }
        // 모든 마나를 소진한 경우 사망처리
        else
        {
            Destroy(gameObject);
        }
    }

    /***************************************************************
    * [ 소생 ]
    * 
    * 마나가 남은 상태에서 사망한 경우 소생
    ***************************************************************/

    private IEnumerator revivalHP()
    {
        // 체력 회복 텀
        WaitForSeconds regenCooldown = new WaitForSeconds(0.1f);

        // 부활 애니메이션
        yield return new WaitForSeconds(2f);

        while (data.Mana > 0 && data.HP < data.MaxHP)
        {
            int regenHP = 1;

            data.Mana -= regenHP;
            data.HP += regenHP;

            yield return regenCooldown;
        }
    }


    /***************************************************************
    * [ 변수 변화 체크 ]
    * 
    * 몬스터의 hp와 mana의 변화에 따른 UI 처리
    ***************************************************************/

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "HP")
        {
            // ui.updateHpText(data.MaxHP, data.HP);

            // 체력이 0이하로 떨어지면 사망 처리
            if (data.HP <= 0)
            {
                OnDead();
            }
            // 현재 체력이 최대 체력보다 낮아지면 리젠 발동
            else if (regenCoroutine == null && data.HP < data.MaxHP)
            {
                regenCoroutine = StartCoroutine(hpRege());
            }
        }

        if (e.PropertyName == "Mana")
        {
            // ui.updateManaText(data.MaxMana, data.Mana);
        }
    }
}