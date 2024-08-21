using UnityEngine;

public enum EffectType
{
    // 버프(이로운 효과)
    Speed,  // 신속: 민첩 수치 증가

    // 디버프(해로운 효과)
    Stun,   // 기절: 행동 불가

}

public class StatusEffect : ScriptableObject
{
    [SerializeField]
    private string _effectName;
    public string EffectName
    {
        get {  return _effectName; }
    }

    [SerializeField]
    private EffectType _type;
    public EffectType Type
    {
        get { return _type; }
    }

    [SerializeField]
    private float _duration;
    public float Duration
    {
        get { return _duration; }
    }

    [SerializeField]
    private float _effectRange;
    public float EffectRange
    {
        get { return _effectRange; }
    }

    [SerializeField]
    [TextArea(0, 3)]
    private string _description;
    public string Description
    {
        get { return _description; }
    }
}