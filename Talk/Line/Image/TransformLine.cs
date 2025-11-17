using UnityEngine;

[System.Serializable]
public class TransformLine : Line
{
    [SerializeField]
    private string _target;
    public string target
    {
        get => _target;

        set => _target = value;
    }

    [SerializeField]
    private Vector2 _pos;
    public Vector2 pos => _pos;

    [SerializeField]
    private Color _color;
    public Color color => _color;

    public TransformLine(Vector2 pos, Color color) : base(LineType.Transform)
    {
        // 鸥百篮 积己等 第 眠啊
        _pos = pos;
        _color = color;
    }

#if UNITY_EDITOR
    public TransformLine(TransformNodeData nodeData) : base(nodeData.guid, LineType.Transform)
    {
        _target = nodeData.targetGuid;
        _pos = nodeData.transPos;
        _color = nodeData.transColor;
    }
#endif
}