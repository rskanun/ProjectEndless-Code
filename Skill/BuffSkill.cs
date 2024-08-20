using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Strength,
}
[CreateAssetMenu(menuName = "Skill/BuffSkill", fileName = "Buff_Skill")]
public class BuffSkill : Skill
{
    [Header("버프 정보")]
    [SerializeField]
    private BuffType _buffType;
    public BuffType BuffType
    {
        get { return _buffType; }
    }
    [SerializeField]
    private float _duration;
    public float Duration
    {
        get { return _duration; }
    }

    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        throw new System.NotImplementedException();
    }
}