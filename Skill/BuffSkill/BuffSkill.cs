using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/BuffSkill", fileName = "Buff_Skill")]
public class BuffSkill : Skill
{
    [Header("버프 정보")]
    [SerializeField]
    private Buff _buff;
    public Buff Buff
    {
        get { return _buff; }
    }

    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        foreach (Entity target in targets)
        {
            target.AddEffect(Buff);
        }
    }
}