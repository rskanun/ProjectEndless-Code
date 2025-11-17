using UnityEngine;

[System.Serializable]
public class ImageLine : Line
{
    [SerializeField]
    private Sprite _sprite;
    public Sprite sprite => _sprite;

    [SerializeField]
    private Vector2 _pos;
    public Vector2 pos => _pos;

    [SerializeField]
    private Color _color;
    public Color color => _color;

    public ImageLine(Sprite sprite, Vector2 pos, Color color) : base(LineType.Image)
    {
        _sprite = sprite;
        _pos = pos;
        _color = color;
    }

#if UNITY_EDITOR
    public ImageLine(ImageNodeData nodeData) : base(nodeData.guid, LineType.Image)
    {
        _sprite = nodeData.sprite;
        _pos = nodeData.spritePos;
        _color = nodeData.color;
    }
#endif
}