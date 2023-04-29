using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class Select : Line
    {
        public List<string> Options { get { return options; } }
        private List<string> options = new List<string>();

        public Select(string[] options) : base(Code.Select)
        {
            // 대사번호(0), 코드(1) 제외
            for (int i = 2; i < options.Length; i++)
            {
                if (options[i].Equals("") == false)
                {
                    this.options.Add(options[i]);
                }
            }
        }
    }
}