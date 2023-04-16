using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Option
{
    public class SliderOption : Option
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        [SerializeField]
        private float _value;
        public float Value { get { return _value; } }
    }
}