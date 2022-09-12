using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class AwakenPointBarUI : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        public Image apBar;

        private float nowAP; // ao의 값이 변경되기 이전의 값

        void Start()
        {
            nowAP = player.ap;
            apBar.fillAmount = (float)player.ap / player.maxAP;
        }
        void Update()
        {
            if(nowAP != player.ap)
            {
                // 깎일 양의 Bar를 10틱으로 나눠 애니메이션의 형태로 보여주기
                StartCoroutine(barAnimation(nowAP, player.ap));

                nowAP = player.ap;
            }
        }

        IEnumerator barAnimation(float before, float after)
        {
            float movePer = (before - after) / 10.0f;
            while (before != after)
            {
                before -= movePer;
                apBar.fillAmount -= movePer / player.maxAP;
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}