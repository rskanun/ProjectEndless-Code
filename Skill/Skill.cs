using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [Header("스킬 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }
    [SerializeField]
    private Sprite _iconSprite;
    public Sprite IconSprite
    {
        get { return _iconSprite; }
    }
    [SerializeField]
    private TargetType _targetType;
    public TargetType TargetType
    {
        get { return _targetType; }
    }
    [SerializeField]
    private float _costTurn;
    public float CostTurn
    {
        get { return _costTurn; }
    }
    [SerializeField]
    private int _costSP;
    public int CostSP
    {
        get { return _costSP; }
    }
    [SerializeField]
    [TextArea(0, 3)]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    public abstract void OnCasting(Entity caster, List<Entity> targets);
}