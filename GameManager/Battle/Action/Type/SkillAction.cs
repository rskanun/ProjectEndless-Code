using System.Collections.Generic;

public class SkillAction : BattleAction
{
    public Skill castSkill;
    private List<Entity> _targets;
    public List<Entity> Targets
    {
        private set { _targets = value; }
        get { return _targets; }
    }

    public SkillAction()
    {
        actionType = ActionType.Skill;
    }

    public override void OnAction()
    {
        actor.OnCast(castSkill, Targets);
    }

    public override void SetTarget(List<Entity> targets)
    {
        Targets = targets.ConvertAll(entity => entity);
    }

    public override TargetType GetTargetType()
    {
        return castSkill.TargetType;
    }
}