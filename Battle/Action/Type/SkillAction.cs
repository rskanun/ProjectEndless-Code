using System;
using System.Collections.Generic;

[Serializable]
public class SkillAction : BattleAction
{
    public Skill castSkill;
    public List<Entity> targets;

    public SkillAction() : base(ActionType.Skill) { }

    public SkillAction(Entity actor, Skill castSkill, List<Entity> targets, float remainTurn) : base(ActionType.Skill)
    {
        this.actor = actor;
        this.castSkill = castSkill;
        this.targets = targets;
        this.remainTurn = remainTurn;
    }

    public override void OnAction()
    {
        actor.CastSkill(castSkill, targets);
    }

    public override List<Entity> GetTargets()
    {
        return targets;
    }

    public override void SetTarget(List<Entity> targets)
    {
        this.targets = targets.ConvertAll(entity => entity);
    }

    public override TargetType GetTargetType()
    {
        return castSkill.TargetType;
    }
}