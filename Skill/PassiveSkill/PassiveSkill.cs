using System.Collections.Generic;

public class PassiveSkill : Skill
{
    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        throw new System.NotImplementedException();
    }
}