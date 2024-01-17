using Assets.Script.System.Option;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private List<Option> optionList;

    private OptionSetting optionSetting;

    private void Awake()
    {
        optionSetting = OptionSetting.Instance;

        //StartCoroutine(checkForControllers());
    }

    private IEnumerator checkForControllers()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
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

    private void readOption()
    {
        // read option to file
    }

    private void writeOption()
    {
        // write option to file
    }

    private void setOptionMenu()
    {
        foreach (Option option in optionList)
        {

        }
    }
}