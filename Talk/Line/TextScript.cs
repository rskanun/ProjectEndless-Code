using System.Collections.Generic;

public class TextScript
{
    private Dictionary<int, List<Line>> _scenarios;

    public TextScript()
    {
        _scenarios = new Dictionary<int, List<Line>>();
    }

    public List<Line> GetLines(int scenarioNum)
    {
        return _scenarios[scenarioNum];
    }

    public void SetLines(List<Line> scenario, int scenarioNum)
    {
        _scenarios[scenarioNum] = scenario;
    }

    public bool ContainsKey(int id)
    {
        return _scenarios.ContainsKey(id);
    }
}