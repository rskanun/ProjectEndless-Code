using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Option
{
    public class ListOption : Option
    {
        [SerializeField]
        private List<string> _toggles;
        public List<string> Toggles { get { return _toggles; } }

        [SerializeField]
        private string _select;
        public string Select { get { return _select;} }
    }
}