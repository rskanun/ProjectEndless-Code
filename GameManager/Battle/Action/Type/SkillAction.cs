public class SkillAction : BattleAction
{
    public Skill castSkill;
    public Entity caster;
    public Entity target;

    public SkillAction()
    {
        action = ActionType.Skill;
    }

    public override BattleAction Clone()
    {
        SkillAction clone = new SkillAction();

        clone.remainTurn = remainTurn;
        clone.castSkill = castSkill;
        clone.caster = caster;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        caster.OnCast(castSkill, target);
    }
}