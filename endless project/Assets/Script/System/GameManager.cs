using System.Collections;
using UnityEngine;

namespace Assets.Script.System
{
    public class GameManager : MonoBehaviour
    {
        private OptionSetting optionSetting;

        private void Awake()
        {
            optionSetting = OptionSetting.Instance;

            allDataLoad();
        }

        private void OnApplicationQuit()
        {
            allDataSave();
        }

        private void allDataLoad()
        {
            optionSetting.load();
        }

        private void allDataSave()
        {
            optionSetting.save();
        }
    }
}