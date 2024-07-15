public class SkillAction : BattleAction
{
    public Skill castSkill;
    public Entity target;

    public SkillAction()
    {
        action = ActionType.Skill;
    }

    public override void OnAction()
    {
        actor.OnCast(castSkill, target);
    }
}