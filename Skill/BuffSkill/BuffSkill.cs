using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/BuffSkill", fileName = "Buff_Skill")]
public class BuffSkill : Skill
{
    [Header("버프 정보")]
    [SerializeField]
    private StatusEffect _buff;
    public StatusEffect Buff
    {
        get { return _buff; }
    }

    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        throw new System.NotImplementedException();
    }
}