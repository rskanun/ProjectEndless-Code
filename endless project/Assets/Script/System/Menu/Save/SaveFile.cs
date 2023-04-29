using Assets.Script.UI.Menu.Save;
using UnityEngine;
using static Assets.Script.UI.Menu.Save.SaveFileUI;

namespace Assets.Script.System.Menu.Save
{
    public class SaveFile
    {
        private int _id;
        public int Id { get { return _id; } }

        private SaveData _data;
        public SaveData Data
        {
            get { return _data; }
        }

        private GameObject saveFileObj;
        private SaveFileUI ui;

        public SaveFile(SaveData data, int id)
        {
            _data = data;
            _id = id;
        }

        public void setObject(GameObject obj)
        {
            saveFileObj = obj;
            ui = saveFileObj.GetComponent<SaveFileUI>();

            // init Object
            ui.setSaveFile(_data.date, _data.location, _data.quest);
        }

        public void setCallBack(SaveFileCallBack Listener)
        {
            ui.setCallBack(Listener);
        }

        public void DestroyObject()
        {
            UnityEngine.Object.Destroy(saveFileObj);
        }
    }
}