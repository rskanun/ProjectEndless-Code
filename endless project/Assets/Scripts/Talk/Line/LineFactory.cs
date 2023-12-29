using System.Linq;

public class LineFactory
{
    private static LineFactory _instance;
    public static LineFactory Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = new LineFactory();
            return _instance;
        }
    }

    public Line CreateLine(LineType lineType, string[] strs)
    {
        switch (lineType)
        {
            case LineType.Text:
                return CreateTextLine(strs);

            case LineType.Select:
                return CreateSelectLine(strs);

            case LineType.Case:
                return CreateCaseLine(strs);

            case LineType.End:
                return CreateEndLine();

            case LineType.Event:
                return CreateEventLine(strs);

            default:
                return null;

        }
    }

    private TextLine CreateTextLine(string[] strs)
    {
        if (strs.Length >= 4)
        {
            string name = strs[2];
            string text = strs[3];

            return new TextLine(name, text);
        }

        else return null;
    }

    private Select CreateSelectLine(string[] strs)
    {
        if (strs.Length >= 3)
        {
            string[] options = strs.Skip(2).ToArray();

            return new Select(options);
        }

        else return null;
    }

    private Case CreateCaseLine(string[] strs)
    {
        if (strs.Length >= 3)
        {
            string choice = strs[2];

            return new Case(choice);
        }

        else return null;
    }

    private Line CreateEndLine()
    {
        return new Line(LineType.End);
    }

    private EventLine CreateEventLine(string[] strs)
    {
        if (strs.Length >= 3)
        {
            string command = strs[2];

            return new EventLine(command);
        }

        else return null;
    }
}