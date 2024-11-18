using UnityEngine;

public class Player : Character
{
    [SerializeField] private AssistAttackManager assistManager;

    public override void OnTargetedAttack(Entity attacker, bool isUsedParry, bool isUsedDodge)
    {
        battleData.IsUsedParry = isUsedParry;
        battleData.IsUsedDodge = isUsedDodge;
    }

    public override void OnParrying()
    {
        // 플레이어가 패링에 성공했을 경우
        BattleAction curAction = battleData.Sequence.GetTurnAction(0);

        // 추가타를 넣을 대상 선택
        assistManager.OnSelectExtraAttacker(curAction.actor);
    }
}