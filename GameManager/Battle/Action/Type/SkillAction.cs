using System.Collections.Generic;

public class SkillAction : BattleAction
{
    public Skill castSkill;
    public List<Entity> targets;

    public SkillAction()
    {
        actionType = ActionType.Skill;
    }

    public override void OnAction()
    {
        actor.OnCast(castSkill, targets);
    }

    public override void SetTarget(List<Entity> targets)
    {
        targets = targets.ConvertAll(entity => entity);
    }

    public override TargetType GetTargetType()
    {
        return castSkill.TargetType;
    }
}