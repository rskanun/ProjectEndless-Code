using System.Collections.Generic;

public class AttackAction : BattleAction
{
    public Entity target;

    public AttackAction()
    {
        actionType = ActionType.Attack;
    }

    public override void OnAction()
    {
        actor.OnAttack(target);
    }

    public override void SetTarget(List<Entity> targets)
    {
        if (targets.Count > 0) target = targets[0];
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