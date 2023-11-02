using System.Collections;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class Case : Line
    {
        public string Choice { get { return choice; } }
        private string choice;

        public Case(string choice) : base(LineType.Case)
        {
            this.choice = choice;
        }
    }
}