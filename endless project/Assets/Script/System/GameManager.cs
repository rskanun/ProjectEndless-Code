using System.Collections;
using UnityEngine;

namespace Assets.Script.System
{
    public class GameManager : MonoBehaviour
    {
        private OptionSetting optionSetting;

        [SerializeField]
        private PlayerData playerData;

        private void Awake()
        {
            optionSetting = OptionSetting.Instance;

            //allDataLoad();

            // player data init
            playerData.Npc = null;
        }

        private void OnApplicationQuit()
        {
            //allDataSave();
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