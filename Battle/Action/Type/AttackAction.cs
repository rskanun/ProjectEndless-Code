using System;
using System.Collections.Generic;

[Serializable]
public class AttackAction : BattleAction
{
    public Entity target;

    public AttackAction() : base(ActionType.Attack) { }

    public AttackAction(Entity actor, Entity target, float remainTurn) : base(ActionType.Attack)
    {
        this.actor = actor;
        this.target = target;
        this.remainTurn = remainTurn;
    }

    public override void OnAction()
    {
        actor.OnAttack(target);
    }

    public override List<Entity> GetTargets()
    {
        return new List<Entity> { target };
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