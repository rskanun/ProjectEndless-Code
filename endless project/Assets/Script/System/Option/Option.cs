using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Option
{
    public enum OptionType
    {
        Slider, Toggle, List
    }

    public class Option : ScriptableObject
    {
        [SerializeField]
        private string _title;
        public string Title { get { return _title; } }

        [SerializeField]
        private OptionType _type;
        public OptionType Type { get { return _type; } }
    }
}