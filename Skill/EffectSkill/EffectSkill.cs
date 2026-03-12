using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/EffectSkill", fileName = "Effect_Skill")]
public class EffectSkill : Skill
{
    [Header("버프 정보")]
    [SerializeReference]
    [ContextMenuItem("Buff", "SetBuff")]
    [ContextMenuItem("Debuff", "SetDebuff")]
    private StatusEffect _effect = new Buff();
    public StatusEffect Effect
    {
        get { return _effect; }
    }

    public void SetBuff()
    {
        _effect = new Buff();
    }

    public void SetDebuff()
    {
        _effect = new Debuff();
    }

    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        foreach (Entity target in targets)
        {
            target.AddEffect(Effect);
        }
    }

    public override string GetTypeName()
    {
        if (_effect is Buff)
            return "버프 스킬";

        return "디버프 스킬";
    }
}