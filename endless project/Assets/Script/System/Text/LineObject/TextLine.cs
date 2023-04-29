using System.Collections;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class TextLine : Line
    {
        public string Name { get { return name; } }
        private string name;
        public string Text { get { return text; } }
        private string text;

        public TextLine(string name, string text) : base(Code.Text)
        {
            this.name = name;
            this.text = text.Replace("\\r\\n", "\r\n");
        }
    }
}