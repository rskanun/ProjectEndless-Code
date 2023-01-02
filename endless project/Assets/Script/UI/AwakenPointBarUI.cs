using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class AwakenPointBarUI : MonoBehaviour
    {
        public Image apBar;

        private float ticks = 10;
        private float spf = 0.025f;

        public void setApBar(float value)
        {
            apBar.fillAmount= value;
        }

        public void barUpdate(float nowAP, Player player)
        {
            StartCoroutine(barAnimation(nowAP, player.ap, player.maxAp));
        }

        IEnumerator barAnimation(float before, float after, float max)
        {
            float movePer = (before - after) / ticks;
            while (before != after)
            {
                before -= movePer;
                apBar.fillAmount -= movePer / max;
                yield return new WaitForSeconds(spf);
            }
        }
    }
}