using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class HealthPointBarUI : MonoBehaviour
    {
        public Image hpBar;

        private const float TICKS = 10;
        private const float SPF = 0.025f; // second per frame

        private float hp;

        public void setHPBar(float value)
        {
            hp = value;
            hpBar.fillAmount = value;
        }

        public void barUpdate(int nowHP, Player player)
        {
            if(hp != player.hp)
            {

            }
            StartCoroutine(barAnimation(nowHP, player.hp, player.maxHp));
        }

        IEnumerator barAnimation(float before, float after, float max)
        {
            float movePer = (before - after) / TICKS;
            WaitForSeconds wait = new WaitForSeconds(SPF);

            while (before != after)
            {
                // 본래 도달해야할 양을 초과했을 경우 or 스킵
                if(movePer > 0 && after > before || movePer < 0 && after < before)
                {
                    before = after;
                    hpBar.fillAmount = after / max;
                }
                else
                {
                    before -= movePer;
                    hpBar.fillAmount -= movePer / max;
                    yield return wait;
                }
            }
        }
    }
}