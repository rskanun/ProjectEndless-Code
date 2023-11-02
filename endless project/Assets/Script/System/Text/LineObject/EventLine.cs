using System.Collections;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class EventLine : Line
    {
        public string Command { get { return command; } }
        private string command;

        public EventLine(string command) : base(LineType.Event)
        {
            this.command = command;
        }
    }
}