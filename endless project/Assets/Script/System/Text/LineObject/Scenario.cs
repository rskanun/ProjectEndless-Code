using Assets.Script.Control.Text.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Text.LineObject
{
    public class Scenario
    {
        private List<Line> _lineList;

        public Scenario()
        {
            _lineList = new List<Line>();
        }

        public void AddLine(Line line)
        {
            if(line != null)
            {
                // null 값이 아닌 line만 추가
                _lineList.Add(line);
            }
        }
    }
}