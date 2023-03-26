using Assets.Script.System;
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
        [Header("참조 스크립트")]
        [SerializeField] private EffectManager effect;

        public void setAPBar(int ap, int maxAP)
        {
            int apPerValue = maxAP / AP_COUNT / (AP_MAX_STEP - 1); // AP의 이미지가 변하는 최소 단위
            int perAP =  ap / apPerValue;

            for (int i = 0; i < AP_COUNT; i++)
            {
                // 이미지 변환
                if (perAP >= (AP_MAX_STEP - 1) * (i + 1)) // 해당 자리에서 최대보다 클 경우 MAX값 변환
                    apBar[i].sprite = apSteps[AP_MAX_STEP - 1];
                else if (perAP < (AP_MAX_STEP - 1) * i) // 해당 자리에서 최저보다 작을 경우 default값 변환
                    apBar[i].sprite = apSteps[0];
                else
                    apBar[i].sprite = apSteps[perAP % (AP_MAX_STEP - 1)];
            }
        }

        public void barUpdate(int ap, int maxAP)
        {
            effect.glitchEffect(0.4f);
            setAPBar(ap, maxAP);
        }
    }
}