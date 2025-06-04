using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AssistAttackManager : MonoBehaviour
{
    [SerializeField] private AssistAttackUI ui;
    [SerializeField] private AssistController controller;
    [SerializeField] private BattleController mainController;

    public float selectTimer;

    // 지원 공격 대상자
    private Entity target;

    // 지원 가능한 캐릭터 목록
    private Entity defender;
    private List<Entity> assistMembers = new List<Entity>();

    public void OnSelectExtraAttacker(Entity target, Entity defender)
    {
        this.target = target;
        this.defender = defender;

        // 시간 배율 조정동안 조작 금지
        ControlContext.Instance.KeyLock();

        // 주인공에게 포커싱을 맞춰 카메라 이동

        // 선택을 고르는 동안 시간 배율이 빠르게 느려짐
        DOTween.To(() => Time.timeScale, t => Time.timeScale = t, 0.05f, 0.8f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() => ActiveAssistSelection());
    }

    private void ActiveAssistSelection()
    {
        ControlContext.Instance.KeyUnlock();

        // 컨트롤러 변경
        ControlContext.Instance.SetController(controller);

        // 타이머 띄우기(타이머 종료 시 플레이어 자동 선택)
        ui.ActiveTimer(selectTimer);

        // 플레이어 선택키 띄우기
        ui.SetActivePlayerSelectIcon(true);

        // 지원 가능한 캐릭터 목록 띄우기
        ActiveAssistableChr(BattleData.Instance.LivingCharacters);
    }

    private void ActiveAssistableChr(List<Entity> livingChrs)
    {
        foreach (Character chr in livingChrs)
        {
            // 경직된 상태일 경우 지원 X
            if (chr.HasState(EntityState.Stun)) continue;

            // 공격을 막아낸 본인은 지원 목록에 추가 X
            if (chr.Equals(defender)) continue;

            // 해당 캐릭터의 다음 행동이 지원 가능 스킬 사용인 경우 나누기

            assistMembers.Add(chr);
        }
    }

    public void OnAssisAttack(int? index = null)
    {
        if (index.HasValue && assistMembers.Count <= index.Value)
        {
            // 지원 가능 맴버 수를 초과한 순서의 맴버 호출 시, 다시 선택
            return;
        }

        // 지원 UI 지우기
        DeactiveAssistSelection();

        // 시간 배율 초기화
        Time.timeScale = 1.0f;

        // 지원 공격
        Entity attacker = index.HasValue ? assistMembers[index.Value] : defender;
        BattleAction action = BattleData.Instance.Sequence.GetEntityAction(attacker);

        if (IsAssistableSkill(action, out SkillAction skillAction))
        {
            skillAction.SetTarget(new List<Entity> { target });

            // 지원 가능한 공격 스킬일 경우 앞당겨 사용
            OnActionAssistSkill(skillAction);
            return;
        }

        // 지원 가능한 스킬이 아닌 경우 지원 공격 사용
        attacker.OnCounterattack(target);
    }

    private void DeactiveAssistSelection()
    {
        // 컨트롤러 변경
        ControlContext.Instance.SetController(mainController);

        // 타이머 종료
        ui.DeactiveTimer();

        // 플레이어 선택키 지우기
        ui.SetActivePlayerSelectIcon(false);

        // 지원가능한 캐릭터 목록 지우기기
        DeactiveAssistableChr();
    }

    private void DeactiveAssistableChr()
    {
        // 지원가능한 캐릭터 목록 지우기기
    }

    private bool IsAssistableSkill(BattleAction action, out SkillAction skillAction)
    {
        skillAction = action as SkillAction;

        // 스킬을 사용하는 행동인지, 그렇다면 해당 스킬이 지원가능한지 여부 리턴
        return skillAction?.castSkill is AttackSkill skill && skill.IsAssistable;
    }

    private void OnActionAssistSkill(SkillAction skillAction)
    {
        BattleSequence sequence = BattleData.Instance.Sequence;

        // 본래 사용할 행동 대신 대기 모션 예약
        sequence.RemoveTurn(skillAction.actor);
        sequence.AddTurn(new WaitAction(skillAction.actor, 0f), 1);

        // 지원 스킬 실행
        skillAction.OnAction();
    }
}