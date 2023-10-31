using UnityEngine;

namespace Assets.Script.System.Menu.App
{
    public class SaveApp : App
    {
        [SerializeField]
        private SaveManager saveManager;

        public override void open()
        {
            base.open();

            saveManager.initSave();
        }
    }
}