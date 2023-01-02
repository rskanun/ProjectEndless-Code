using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class HealthPointBarUI : MonoBehaviour
    {
        public Image hpBar;

        private float ticks = 10;
        private float spf = 0.025f;

        public void setHpBar(float value)
        {
            hpBar.fillAmount = value;
        }

        public void barUpdate(float nowHP, Player player)
        {
            StartCoroutine(barAnimation(nowHP, player.hp, player.maxHp));
        }

        IEnumerator barAnimation(float before, float after, float max)
        {
            float movePer = (before - after) / ticks;
            while (before != after)
            {
                before -= movePer;
                hpBar.fillAmount -= movePer / max;
                yield return new WaitForSeconds(spf);
            }
        }
    }
}