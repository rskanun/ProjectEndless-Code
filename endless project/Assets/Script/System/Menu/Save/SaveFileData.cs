using UnityEditor;
using UnityEngine;

namespace Assets.Script.System.Menu.Save
{
    public class SaveFileData
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private SaveData _data;
        public SaveData Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}