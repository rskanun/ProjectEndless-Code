using System.Linq;

public static class LineFactory
{
    public static Line CreateLine(LineType lineType, string[] strs)
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

    private static TextLine CreateTextLine(string[] strs)
    {
        if (strs.Length >= 4)
        {
            string name = strs[2];
            string text = strs[3];

            return new TextLine(name, text);
        }

        return null;
    }

    private static Select CreateSelectLine(string[] strs)
    {
        if (strs.Length >= 3)
        {
            string[] options = strs.Skip(2).ToArray();

            return new Select(options);
        }

        return null;
    }

    private static Case CreateCaseLine(string[] strs)
    {
        if (strs.Length >= 3)
        {
            string choice = strs[2];

            return new Case(choice);
        }

        return null;
    }

    private static Line CreateEndLine()
    {
        return new Line(LineType.End);
    }

    private static EventLine CreateEventLine(string[] strs)
    {
        if (strs.Length >= 3)
        {
            string command = strs[2];

            return new EventLine(command);
        }

        return null;
    }
}