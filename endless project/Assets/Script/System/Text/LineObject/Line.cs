using System.Collections;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class Line
    {
        private LineType code;
        public LineType Code { get { return code; } }

        public Line(LineType code)
        {
            this.code = code;
        }
    }
}