using System.Collections;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class Line
    {
        private Code code;
        public Code Code { get { return code; } }

        public Line(Code code)
        {
            this.code = code;
        }
    }
}