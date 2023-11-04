using Assets.Script.Control.Text.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Text.LineObject
{
    public class Script
    {
        private Dictionary<int, List<Line>> _scenarios;

        public Script()
        {
            _scenarios = new Dictionary<int, List<Line>>();
        }

        public List<Line> getScenario(int scenarioNum)
        {
            return _scenarios[scenarioNum];
        }

        public void setScenario(List<Line> scenario, int scenarioNum)
        {
            _scenarios[scenarioNum] = scenario;
        }

        public bool ContainsKey(int id)
        {
            return _scenarios.ContainsKey(id);
        }
    }
}