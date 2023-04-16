using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Option
{
    public class ToggleOption : Option
    {
        [SerializeField]
        private bool _value;
        public bool Value { get { return _value; } }
    }
}