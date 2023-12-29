public class TextLine : Line
{
    public string name { get { return _name; } }
    private string _name;
    public string text { get { return _text; } }
    private string _text;

    public TextLine(string name, string text) : base(LineType.Text)
    {
        _name = name;
        _text = text.Replace("\\r\\n", "\r\n");
    }
}