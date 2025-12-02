using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[System.Serializable]
public class ScenarioScene : IEnumerable
{
    [SerializeField]
    private List<Line> lines = new();

    public ScenarioScene(List<Line> lines)
    {
        this.lines = lines;
    }

    public IEnumerator GetEnumerator()
    {
        return new LineEnumerator(lines.FirstOrDefault());
    }
}

public class LineEnumerator : IEnumerator
{
    private Line introLine;
    private Line currentLine;
    private int nextIndex;
    public object Current => currentLine;

    public LineEnumerator(Line introLine)
    {
        this.introLine = introLine;
        currentLine = introLine;
    }

    public bool MoveNext()
    {
        if (currentLine == null || currentLine.nextLines.Count >= nextIndex)
        {
            return false;
        }

        currentLine = currentLine.nextLines[nextIndex];
        nextIndex = 0;

        return currentLine != null;
    }

    public void Reset()
    {
        currentLine = introLine;
        nextIndex = 0;
    }
}