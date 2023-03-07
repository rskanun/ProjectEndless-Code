using UnityEditor;
using UnityEngine;

namespace Assets.Script.Item
{
    public enum Type
    {
        Consumable,
        Weapon,
        Armor,
        Miscellaneous,
        Quest
    }

    public class Item : ScriptableObject
    {
        [SerializeField]
        private string itmeName;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [SerializeField]
        private Type type;
        public Type ItemType
        {
            get { return type; }
            set { type = value; }
        }

        [SerializeField]
        private string[] lores;
        public string[] Lores
        {
            get { return lores; }
            set { lores = value; }
        }
    }
}