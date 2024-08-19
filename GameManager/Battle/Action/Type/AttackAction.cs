using System.Collections.Generic;

public class AttackAction : BattleAction
{
    private Entity _target;
    public Entity Target
    {
        private set { _target = value; }
        get { return _target; }
    }

    public AttackAction()
    {
        actionType = ActionType.Attack;
    }

    public override void OnAction()
    {
        actor.OnAttack(Target);
    }

    public override void SetTarget(List<Entity> targets)
    {
        if (targets.Count > 0) Target = targets[0];
    }

    public override TargetType GetTargetType()
    {
        bool isEnemy = actor is Monster;
        bool isMelee = actor.AttackType == AttackType.Melee;

        if (isEnemy)
        {
            return isMelee ? TargetType.FrontMember : TargetType.Member;
        }
        else
        {
            return isMelee ? TargetType.FrontEnemy : TargetType.Enemy;
        }
    }
}