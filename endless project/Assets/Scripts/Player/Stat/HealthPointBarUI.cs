using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthPointBarUI : MonoBehaviour
{
    public Image hpBar;

    private float ticks = 10;
    private float spf = 0.025f; // second per frame

    private float currentHp;
    private float maxHp;

    private Coroutine animationCoroutine;

    public void SetHpBar(int hp, int maxHp)
    {
        this.currentHp = hp;
        this.maxHp = maxHp;

        hpBar.fillAmount = (float)hp / maxHp;
    }

    public void BarUpdate(int beforeHP, float setHP)
    {
        // 코루틴이 진행 도중이면 이전 코루틴 스탑
        if (currentHp != beforeHP)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(BarAnimation(setHP, maxHp));
    }

    private IEnumerator BarAnimation(float setHP, float maxHP)
    {
        float movePer = (currentHp - setHP) / ticks;
        WaitForSeconds wait = new WaitForSeconds(spf);

        while (currentHp != setHP)
        {
            // 본래 도달해야할 양을 초과했을 경우
            if (movePer > 0 && setHP > currentHp || movePer < 0 && setHP < currentHp)
            {
                currentHp = setHP;
                hpBar.fillAmount = setHP / maxHP;
            }
            else
            {
                currentHp -= movePer;
                hpBar.fillAmount -= movePer / maxHP;
                yield return wait;
            }
        }
    }
}