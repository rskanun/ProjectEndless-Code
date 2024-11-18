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
    private List<Entity> assistMembers = new List<Entity>();

    public void OnSelectExtraAttacker(Entity target)
    {
        this.target = target;

        // 시간 배율 조정동안 조작 금지
        ControlContext.Instance.KeyLock();

        // 주인공에게 포커싱을 맞춰 카메라 이동

        // 선택을 고르는 동안 시간 배율이 빠르게 느려짐
        DOTween.To(() => Time.timeScale, t => Time.timeScale = t, 0.001f, 1f)
            .SetEase(Ease.OutQuint)
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
        ActiveAssistableChr(CurrentBattleData.Instance.LivingCharacters);
    }

    private void ActiveAssistableChr(List<Entity> livingChrs)
    {
        foreach (Character chr in livingChrs)
        {
            // 경직된 상태일 경우 지원 X
            if (chr.HasStun) return;

            // 해당 캐릭터의 다음 행동이 지원 가능 스킬 사용인 경우 나누기

            assistMembers.Add(chr);
        }
    }

    public void OnAssisAttack(int index)
    {
        // 기존 컨트롤러로 다시 변경
        ControlContext.Instance.SetController(mainController);

        // 시간 배율 초기화
        Time.timeScale = 1.0f;

        // 지원 공격
        Entity attacker = assistMembers[index];

        attacker.OnAssistAttack(target);
    }
}