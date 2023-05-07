using UnityEngine;

namespace Assets.Script.System.Interface.Menu.App
{
    public class LoadApp : App
    {
        [SerializeField]
        private SaveManager saveManager;

        public override void open()
        {
            base.open();

            saveManager.initLoad();
        }
    }
}