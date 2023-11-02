using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Text.LineObject
{
    public class Script
    {
        private Dictionary<int, Scenario> _scenarios;

        public Script()
        {
            _scenarios = new Dictionary<int, Scenario>();
        }

        public Scenario getScenario(int scenarioNum)
        {
            return _scenarios[scenarioNum];
        }

        public void setScenario(Scenario scenario, int scenarioNum)
        {
            _scenarios[scenarioNum] = scenario;
        }

    }
}