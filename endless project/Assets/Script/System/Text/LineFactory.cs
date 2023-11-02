using Assets.Script.Control.Text;
using Assets.Script.Control.Text.Object;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Text
{
    public class LineFactory
    {
        private static LineFactory _instance;
        public static LineFactory Instance
        {
            get
            {
                if(_instance != null) return _instance;

                _instance = new LineFactory();
                return _instance;
            }
        }

        public Line createLine(LineType lineType, string[] strs)
        {
            switch (lineType)
            {
                case LineType.Text:
                    return createTextLine(strs);

                case LineType.Select: 
                    return createSelectLine(strs);

                case LineType.Case:
                    return createCaseLine(strs);

                case LineType.End:
                    return createEndLine();

                case LineType.Event:
                    return createEventLine(strs);

                default:
                    return null;

            }
        }

        private TextLine createTextLine(string[] strs)
        {
            if (strs.Length >= 4)
                return new TextLine(strs[2], strs[3]);

            else return null;
        }

        private Select createSelectLine(string[] strs)
        {
            if (strs.Length >= 3)
                return new Select(strs);

            else return null;
        }

        private Case createCaseLine(string[] strs)
        {
            if (strs.Length >= 3)
                return new Case(strs[2]);

            else return null;
        }

        private Line createEndLine()
        {
            return new Line(LineType.End);
        } 

        private EventLine createEventLine(string[] strs)
        {
            if(strs.Length >= 3)
                return new EventLine(strs[2]);

            else return null;
        }
    }
}