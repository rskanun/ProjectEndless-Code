public class SkillAction : BattleAction
{
    public Skill castSkill;
    public Entity caster;
    public Entity target;

    public SkillAction(float turn)
    {
        remainTurn = turn;
        action = ActionType.Skill;
    }

    public override BattleAction Clone()
    {
        SkillAction clone = new SkillAction(remainTurn);

        clone.castSkill = castSkill;
        clone.caster = caster;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        castSkill.OnCasting(target);
    }
}