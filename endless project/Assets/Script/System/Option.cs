using System.Collections;
using UnityEngine;

namespace Assets.Script.System
{
    public class Option : MonoBehaviour
    {
        private OptionSetting optionSetting;

        private void Awake()
        {
            optionSetting = OptionSetting.Instance;
            NoKeyDown.Instance.initialize();

            StartCoroutine(checkForControllers());
        }

        private void OnApplicationQuit()
        {
            NoKeyDown.Instance.initialize();
        }

        private IEnumerator checkForControllers()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);

            while(true)
            {
                var controllers = Input.GetJoystickNames();

                if (optionSetting.IsController == false && controllers.Length > 0)
                {
                    optionSetting.IsController = true;
                }
                else if (optionSetting.IsController && controllers.Length == 0)
                {
                    optionSetting.IsController = false;
                }

                yield return wait;
            }
        }
    }
}