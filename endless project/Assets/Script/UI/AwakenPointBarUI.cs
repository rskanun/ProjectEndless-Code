using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class AwakenPointBarUI : MonoBehaviour
    {
        private const int AP_COUNT = 5; // AP 갯수
        private const int AP_MAX_STEP = 5; // AP 변화 단계

        [SerializeField]
        private Image[] apBar = new Image[AP_COUNT];
        public Sprite[] apSteps = new Sprite[AP_MAX_STEP]; // 각각의 변화 단계 이미지

        [Space]
        [Header("글리치 이펙트")]
        public GameObject glitch;

        public void setAPBar(Player player)
        {
            int apPerValue = player.maxAp / AP_COUNT / (AP_MAX_STEP - 1); // AP의 이미지가 변하는 최소 단위
            int ap = player.ap / apPerValue;

            for (int i = 0; i < AP_COUNT; i++)
            {
                // 이미지 변환
                if (ap >= (AP_MAX_STEP - 1) * (i + 1)) // 해당 자리에서 최대보다 클 경우 MAX값 변환
                    apBar[i].sprite = apSteps[AP_MAX_STEP - 1];
                else if (ap < (AP_MAX_STEP - 1) * i) // 해당 자리에서 최저보다 작을 경우 default값 변환
                    apBar[i].sprite = apSteps[0];
                else
                    apBar[i].sprite = apSteps[ap % (AP_MAX_STEP - 1)];
            }
        }

        public void barUpdate(Player player)
        {
            StartCoroutine(glitchEffect());
            setAPBar(player);
        }

        IEnumerator glitchEffect()
        {
            glitch.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            glitch.SetActive(false);
        }
    }
}