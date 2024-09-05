using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    // 버프(이로운 효과)
    Haste,  // 신속: 민첩 수치 증가

    // 디버프(해로운 효과)
    Stun,   // 기절: 행동 불가

}

[System.Serializable]
public class StatusEffectData
{
    [SerializeField]
    private EffectType _type;
    public EffectType Type
    {
        get { return _type; }
    }

    [SerializeField]
    private float _effectRange;
    public float EffectRange
    {
        get { return _effectRange; }
    }
}

[System.Serializable]
public class StatusEffect
{
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon
    {
        get { return _icon; }
    }

    [SerializeField]
    private bool _isBuff;
    public bool IsBuff
    {
        get { return _isBuff; }
    }

    [SerializeField]
    private List<StatusEffectData> _effects;
    public List<StatusEffectData> Effects
    {
        get { return _effects; }
    }

    [SerializeField]
    private float _duration;
    public float Duration
    {
        get { return _duration; }
    }

    [SerializeField]
    [TextArea(0, 3)]
    private string _description;
    public string Description
    {
        get { return _description; }
    }
}