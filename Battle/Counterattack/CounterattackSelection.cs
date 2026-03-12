using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CounterattackSelection : MonoBehaviour
{
    [SerializeField] private CounterattackUI ui;
    [SerializeField] private CounterController controller;
    [SerializeField] private BattleController mainController;

    public float selectTimer;

    // 반격하는 캐릭터
    private const int DefenderIdx = 0; // 0번째는 공격을 막아낸 대상자
    private int attackerIdx = -1; // -1은 대상을 고르지 않음을 나타냄

    public void SelectAttacker(int index)
    {
        attackerIdx = index;
    }

    public void ShowAttackerSelection(Entity target, Entity defender)
    {
        StartCoroutine(ActiveSelection(target, defender));
    }

    private IEnumerator ActiveSelection(Entity target, Entity defender)
    {
        // 모든 UI가 활성화 될 때까지 조작 금지
        ControlContext.Instance.KeyLock();

        // 점점 시간이 느려지는 연출
        yield return DOTween.To(() => Time.timeScale, t => Time.timeScale = t, 0.05f, 0.8f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .WaitForCompletion();

        // 타이머 띄우기(타이머 종료 시 플레이어 자동 선택)
        ui.ActiveTimer(selectTimer);

        // 플레이어 선택키 띄우기
        ui.SetActivePlayerSelectIcon(true);

        // 패링 대상 외 주변에서 반격 가능한 캐릭터 목록 띄우기
        List<Entity> assistableChrs = GetAssistableChrs(BattleData.Instance.LivingCharacters);
        ui.ActiveAssistableChrs(assistableChrs);

        // 컨트롤러 변경
        ControlContext.Instance.SetController(controller);

        // 조작 금지 해제
        ControlContext.Instance.KeyUnlock();

        // 반격할 캐릭터를 고를 때까지 대기 
        yield return new WaitUntil(() => 0 <= attackerIdx && attackerIdx <= assistableChrs.Count);

        // 시간 원래대로 되돌리기
        Time.timeScale = 1.0f;

        // 컨트롤러 변경
        ControlContext.Instance.SetController(mainController);

        // 타이머 종료
        ui.DeactiveTimer();

        // 플레이어 선택키 지우기
        ui.SetActivePlayerSelectIcon(false);

        // 지원가능한 캐릭터 목록 지우기기
        ui.DeactiveAssistableChrs();

        // 반격 실행
        // 0 -> 공격을 막아낸 대상자, 1~ -> 지원 가능한 캐릭터
        if (attackerIdx == DefenderIdx) PerformCounterattack(defender, target);
        else PerformCounterSkill(assistableChrs[attackerIdx - 1], target); // 배열은 0부터 시작
    }

    private List<Entity> GetAssistableChrs(List<Entity> livingChrs)
    {
        return livingChrs.Where(chr => IsAssistableChr(chr)).ToList();
    }

    private bool IsAssistableChr(Entity entity)
    {
        // 현재 캐릭터가 적의 공격을 막아낸 대상이 아닌 경우
        // 움직일 수 있고, 다음 행동이 반격 가능 스킬을 가지고 있는 지 확인
        return !entity.HasState(EntityState.Stun) && HasAssistableSkill(entity, out _);
    }

    private void PerformCounterattack(Entity attacker, Entity target)
    {
        attacker.Counterattack(target);
    }

    private void PerformCounterSkill(Entity attacker, Entity target)
    {
        if (!HasAssistableSkill(attacker, out SkillAction skillAction)) return;

        BattleSequence sequence = BattleData.Instance.Sequence;

        // 스킬 앞당겨 사용
        skillAction.SetTarget(new List<Entity> { target });

        // 본래 사용할 행동 대신 대기 모션 예약
        sequence.RemoveTurn(skillAction.actor);
        sequence.AddTurn(new WaitAction(skillAction.actor, 0f), 1);

        // 지원 스킬 실행
        skillAction.OnAction();
    }

    private bool HasAssistableSkill(Entity entity, out SkillAction skillAction)
    {
        BattleAction action = BattleData.Instance.Sequence.GetEntityAction(entity);

        skillAction = action as SkillAction;

        // 스킬을 사용하는 행동인지, 그렇다면 해당 스킬이 지원가능한지 여부 리턴
        return skillAction?.castSkill is AttackSkill skill && skill.IsAssistable;
    }
}