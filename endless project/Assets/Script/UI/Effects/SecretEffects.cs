using Assets.Script.System;
using Mono.Cecil;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Time = Assets.Script.System.Option.Time;

namespace Assets.Script.UI.Effects
{
    public class SecretEffects : MonoBehaviour
    {
        [Space]
        [Header("오브젝트")]
        [SerializeField] private GameObject timePanel;
        [SerializeField] private GameObject timeText;
        [Header("텍스트")]
        [SerializeField] private TextMeshProUGUI hourText;
        [SerializeField] private TextMeshProUGUI minuteText;
        [SerializeField] private TextMeshProUGUI secondText;
        [SerializeField] private TextMeshProUGUI colonText;

        private NoKeyDown noKeyDown;
        private OptionSetting option;

        private void Awake()
        {
            noKeyDown = NoKeyDown.Instance;
            option = OptionSetting.Instance;
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.V))
            {
                secretEffect();
            }
        }

        public void secretEffect()
        {
            int blinkTime = 4;
            float pauseTime = 0.6f;

            StartCoroutine(timeSubEffect(option.TimeSetting, blinkTime, pauseTime));
        }

        private void timePanelActive(bool isActive)
        {
            noKeyDown.IsPlayerControllable = !isActive;

            timePanel.SetActive(isActive);
            timeText.SetActive(isActive);
        }

        IEnumerator timeSubEffect(Time time, int blinkCount, float pauseTime)
        {
            WaitForSeconds wait = new WaitForSeconds(pauseTime);

            timeSet(time);
            timePanelActive(true);

            // blink colon
            for (int i = 0; i < blinkCount * 2; i++)
            {
                if (i % 2 == 0)
                    colonText.gameObject.SetActive(true);
                else
                    colonText.gameObject.SetActive(false);

                yield return wait;
            }

            // time sub
            time--;
            timeSet(time);

            yield return wait;

            timePanelActive(false);
        }

        private void timeSet(Time time)
        {
            hourText.text = time.Hour.ToString();
            minuteText.text = time.Minute.ToString();
            secondText.text = time.Second.ToString();

            colonText.gameObject.SetActive(true);
        }
    }
}