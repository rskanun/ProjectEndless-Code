public class Case : Line
{
    public string Choice { get { return choice; } }
    private string choice;

    public Case(string choice) : base(LineType.Case)
    {
        this.choice = choice;
    }
}