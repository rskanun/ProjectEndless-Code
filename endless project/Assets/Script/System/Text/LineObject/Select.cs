using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Control.Text.Object
{
    public class Select : Line
    {
        private List<string> options = new List<string>();
        public List<string> Options { get { return options; } }

        // key - 선택지, value - 해당 선택지의 케이스 라인 넘버
        private Dictionary<string, int> optionsLineNum = new Dictionary<string, int>();
        public Dictionary<string, int> OptionsLineNum { get { return optionsLineNum; } }

        private int endLineNum = 0;
        public int EndLineNum
        {
            get { return endLineNum; }
            set
            {
                if(endLineNum == 0) endLineNum = value;
            }
        }

        public Select(string[] options) : base(LineType.Select)
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

        public void addOptionBookmark(string option, int lineNum)
        {
            optionsLineNum[option] = lineNum;
        }
    }
}