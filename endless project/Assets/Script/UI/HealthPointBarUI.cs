using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class HealthPointBarUI : MonoBehaviour
    {
        public Image hpBar;

        private float ticks = 10;
        private float spf = 0.025f; // second per frame

        private float nowHP;

        private Coroutine animationCoroutine;

        public void setHPBar(int hp, int maxHp)
        {
            nowHP = hp;
            hpBar.fillAmount = (float)hp / maxHp;
        }

        public void barUpdate(int beforeHP, float setHP, float maxHP)
        {
            // 코루틴이 진행 도중이면 이전 코루틴 스탑
            if (nowHP != beforeHP)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(barAnimation(setHP, maxHP));
        }

        IEnumerator barAnimation(float setHP, float maxHP)
        {
            float movePer = (nowHP - setHP) / ticks;
            WaitForSeconds wait = new WaitForSeconds(spf);

            while (nowHP != setHP)
            {
                // 본래 도달해야할 양을 초과했을 경우
                if(movePer > 0 && setHP > nowHP || movePer < 0 && setHP < nowHP)
                {
                    nowHP = setHP;
                    hpBar.fillAmount = setHP / maxHP;
                }
                else
                {
                    nowHP -= movePer;
                    hpBar.fillAmount -= movePer / maxHP;
                    yield return wait;
                }
            }
        }
    }
}