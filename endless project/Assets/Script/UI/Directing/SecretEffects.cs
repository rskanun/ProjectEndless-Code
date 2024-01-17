using Assets.Script.System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Effects
{
    public class SecretEffects : MonoBehaviour
    {
        [Space]
        [Header("오브젝트")]
        [SerializeField] private GameObject timePanel;
        [SerializeField] private TextMeshProUGUI timeText;
        [Header("참조 스크립트")]
        [SerializeField] private EffectManager effect;

        private OptionSetting option;

        private string originTime
        {
            get 
            {
                return option.Hour + ":" + option.Minute + ":" + option.Second; 
            }
        }
        private string blinkTime
        {
            get
            {
                return option.Hour + " " + option.Minute + " " + option.Second;
            }
        }

        private static SecretEffects _instance;
        public static SecretEffects Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            _instance = this;

            option = OptionSetting.Instance;
        }

        /************************************************************
        * [■■ 연출]
        * 
        * ■■ 연출 관련 함수
        ************************************************************/

        public void play()
        {
            int blinkTime = 3;
            float pauseTime = 0.6f;

            StartCoroutine(timeSubDirecting(blinkTime, pauseTime));
        }

        IEnumerator timeSubDirecting(int blinkCount, float pauseTime)
        {
            string originStr = originTime;
            string blinkStr = blinkTime;

            WaitForSeconds wait = new WaitForSeconds(pauseTime);

            timePanelActive(true);

            // blink colon
            for (int i = 0; i < blinkCount * 2; i++)
            {
                if (i % 2 == 0)
                    timeText.text = originStr;
                else
                    timeText.text = blinkStr;

                yield return wait;
            }

            effect.GlitchEffect(pauseTime / 2);

            yield return wait;

            effect.GlitchEffect(pauseTime / 2);

            // time sub
            option.timeSub();
            timeText.text = originTime;

            yield return wait;

            timePanelActive(false);
        }

        private void timePanelActive(bool isActive)
        {
            // playerState.IsPlayerControllable = !isActive;

            timeText.gameObject.SetActive(isActive);
            timePanel.SetActive(isActive);
        }
    }
}