using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurveyManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SurveyUI ui;
    [SerializeField] private Timeline timeline;
    [SerializeField] private SurveyController thisController;
    [SerializeField] private BattleController mainController;

    private CurrentBattleData battleData;
    private BattleSequence seq;

    private int prevIndex = -1;
    private List<GameObject> arrows = new List<GameObject>();

    private void Awake()
    {
        battleData = CurrentBattleData.Instance;
        seq = battleData.Sequence;
    }

    public void OnStartSurvey()
    {
        // 컨트롤러 셋팅
        mainController.SetSubController(thisController);

        // 첫 타임라인부터 살피기
        SurveyingAction(0);
    }

    public void OnEndSurvey()
    {
        // 조회 정보 삭제
        RemovePrevInfo();

        // 이전 인덱스 값 초기화
        prevIndex = -1;

        // 타임라인 정상화
        timeline.MoveIndex(0);

        // 컨트롤러 해제
        mainController.SetSubController(null);
    }

    public void SurveyNext()
    {
        // 타임라인 옮기기
        timeline.MoveNext();

        // 현재 샌터에 위치한 타임라인 행동 보이기
        SurveyingAction(timeline.CenterIndex);
    }

    public void SurveyPrev()
    {
        // 타임라인 옮기기
        timeline.MovePrev();

        // 현재 샌터에 위치한 타임라인 행동 보이기
        SurveyingAction(timeline.CenterIndex);
    }

    private void SurveyingAction(int index)
    {
        if (prevIndex == index)
        {
            // 이전 값과 동일한 경우 살피기 고정
            return;
        }

        // 이전 내역 삭제
        RemovePrevInfo();

        // 이전 인덱스 값에 현재 인덱스 할당
        prevIndex = index;

        // 해당 행동 화면상에 띄우기
        BattleAction action = seq.GetTurnAction(index);

        ActiveActionIcon(action.actor, action.ActionType);  // 현재 행동 표시
        CreateTargetingArrow(action);   // 타겟 또는 타겟으로 가능한 엔티티 표시
        ActiveForecastHP(action);   // 예상 체력 표시
        ViewForecastEffect(action);     // 예상 상태효과 표시
    }

    private void RemovePrevInfo()
    {
        if (prevIndex < 0)
        {
            // 이전 값이 없다면 삭제 X
            return;
        }

        // 이전 정보 삭제
        BattleAction action = seq.GetTurnAction(prevIndex);

        DeactiveActionIcon(action.actor);
        ClearArrows();
        DeactiveForecastHP(action.GetTargets());
        HideForecaseEffect(action.GetTargets());
    }

    /***************************************************************
    * [ 행동 표시 ]
    * 
    * 현재 살피는 행동이 어떤 행동인지 아이콘으로 띄우기
    ***************************************************************/

    private void ActiveActionIcon(Entity actor, ActionType type)
    {
        actor.ActiveActionIcon(type);
    }

    private void DeactiveActionIcon(Entity actor)
    {
        actor.HideActionIcon();
    }

    /***************************************************************
    * [ 타겟 표시 ]
    * 
    * 현재 살피는 행동의 타겟이 누구인지 또는 누가 될 수 있는 지
    * 포물선 모양의 점선 화살표로 띄우기
    ***************************************************************/

    private void CreateTargetingArrow(BattleAction action)
    {
        List<Entity> targets = GetTargets(action);
        if (targets == null)
        {
            // 타겟을 선택하지 않는 행동일 경우 화살표 생성 X
            return;
        }

        // 행동자 및 타겟 위치 가져오기
        Vector2 actorPos = action.actor.transform.position;
        List<Vector2> targetsPos = targets.Select(t => (Vector2)t.transform.position).ToList();

        CreateArrows(actorPos, targetsPos);
    }

    private List<Entity> GetTargets(BattleAction action)
    {
        // 행동자가 적 진형이라면 타겟팅이 가능한 인물만 보이기
        if (action.actor is Monster)
            return GetTargetableEntities(action.GetTargetType());

        // 행동자가 플레이어 진형이라면 타겟 보여주기
        return action.GetTargets();
    }

    private List<Entity> GetTargetableEntities(TargetType type)
    {
        List<Entity> targetableEntities = type switch
        {
            // 전위가 살아있다면 전위만을 리턴
            TargetType.FrontMember when battleData.IsLivingCharacterFront =>
                battleData.CharacterFrontList.Select(chr => (Entity)chr).ToList(),

            // 전위가 죽었다면 캐릭터를 선택하는 모든 타입은 캐릭터 리턴
            TargetType.FrontMember or TargetType.Member or TargetType.PlayerParty =>
                battleData.CharacterList.Select(chr => (Entity)chr).ToList(),

            // 적을 선택하는 모든 타입은 적을 리턴
            TargetType.Enemy or TargetType.EnemyParty =>
                battleData.EnemyList.Select(enemy => (Entity)enemy).ToList(),

            // 나머지 타입은 선택할 수 있는 적이 없음
            _ => null
        };

        return targetableEntities;
    }

    private void CreateArrows(Vector2 actor, List<Vector2> targets)
    {
        if (targets == null || targets.Count <= 0)
        {
            // 선택된 타겟이 없으면 생성 X
            return;
        }

        // 새 화살표 생성
        foreach (Vector2 target in targets)
        {
            // 본인으로부터 본인에게 향하는 화살표 제외
            if (target != actor)
            {
                GameObject arrow = ui.CreateArrow(actor, target);

                arrows.Add(arrow);
            }
        }
    }

    private void ClearArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }

        arrows.Clear();
    }

    /***************************************************************
    * [ 예상 체력 표시 ]
    * 
    * 현재 살피는 행동으로 인해 깎이거나 깎인 체력 표시
    ***************************************************************/

    private void ActiveForecastHP(BattleAction action)
    {
        // 예상 체력 계산
        List<Entity> targets = action.GetTargets();

        if (action.actor is Monster || targets == null)
        {
            // 행동하는 대상이 적이거나 타겟이 없는 경우 체력 표시 X
            return;
        }

        float attackDmg = GetAttackDmg(action);
        foreach (Entity target in targets)
        {
            int lastDmg = target.GetLastDmg(attackDmg);

            // 현재 턴인 경우 이번 행동에 대한 결과 이전 값을 출력
            if (seq.GetTurnAction(0) != action) target.SetForecastHP(-lastDmg);
            else target.SetForecastHP(lastDmg);
        }
    }

    private void DeactiveForecastHP(List<Entity> targets)
    {
        if (targets == null)
        {
            // 해당 행동에 선택 가능한 타겟이 없다면 리턴
            return;
        }

        foreach (Entity target in targets)
        {
            // 타겟 하나하나의 예상 체력 비활성화
            target.SetActiveForecastHP(false);
        }
    }

    private float GetAttackDmg(BattleAction action)
    {
        if (action is AttackAction)
        {
            // 일반 공격은 해당 캐릭터의 자체 데미지 가져오기
            return action.actor.AttackDmg;
        }
        else if (action is SkillAction)
        {
            SkillAction skillAction = (SkillAction)action;
            AttackSkill skill = skillAction.castSkill as AttackSkill;

            if (skill != null)
            {
                // 공격 스킬만 데미지 계산
                return skill.GetSkillDmg(action.actor);
            }
        }

        // 나머지 행동은 데미지 X
        return 0.0f;
    }

    /***************************************************************
    * [ 예상 상태효과 표시 ]
    * 
    * 현재 살피는 행동으로 인해 생기거나 생긴 상태효과 표시
    ***************************************************************/

    public void ViewForecastEffect(BattleAction action)
    {
        List<Entity> targets = action.GetTargets();
        StatusEffect effect = GetStatusEffect(action);

        if (effect == null)
        {
            // 버프 스킬 혹은 소모 아이템만 적용
            return;
        }

        ActiveForecastEffect(targets, effect);
    }

    private StatusEffect GetStatusEffect(BattleAction action)
    {
        switch (action)
        {
            case SkillAction skillAction when skillAction.castSkill is EffectSkill skill:
                return skill.Effect;

            case ItemAction itemAction:
                return itemAction.usingItem.Effect;

            default:
                return null;
        }
    }

    private void ActiveForecastEffect(List<Entity> targets, StatusEffect effect)
    {
        foreach (Entity target in targets)
        {
            target.SetForecastEffect(effect);
        }
    }

    public void HideForecaseEffect(List<Entity> targets)
    {
        if (targets == null)
        {
            // 타겟이 없다면 스킵
            return;
        }

        foreach (Entity target in targets)
        {
            target.ClearForecastEffect();
        }
    }
}