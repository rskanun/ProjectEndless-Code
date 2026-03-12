using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SelectLine : Line
{
    [SerializeField]
    private List<string> _options = new();
    public List<string> options => _options;

    [SerializeField]
    private List<string> _optionKeys = new();
    public List<string> optionKeys => _optionKeys;

    public SelectLine(string[] options) : base(LineType.Select)
    {
        _options = new List<string>(options);
    }

#if UNITY_EDITOR
    public SelectLine(SelectNodeData nodeData) : base(nodeData.guid, LineType.Select)
    {
        _options = nodeData.options;
        _optionKeys = nodeData.optionKeys;
    }
#endif
}