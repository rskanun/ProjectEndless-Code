using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    // 참조 스크립트
    [SerializeField] private ActionManager actionSelection;
    [SerializeField] private AssistAttackManager assistManager;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnJoinBattle()
    {
        // 본인의 데이터를 파티데이터에서 가져옴
        CharacterData data = PartyData.Instance.GetCharacter(Name);

        // 데이터 덮어씌우기
        Position = data.Position;
        AttackType = data.AttackType;
        PersonalityType = data.Personality;
        SkillList = data.Skills;
        OriginStat = data.Stat;

        // 최종스텟 설정
        Stat = OriginStat.Clone();

        // HUD 설정
        InitHUD();

        // 오브젝트 활성화
        gameObject.SetActive(true);
    }

    private void InitHUD()
    {
        // 해당 캐릭터의 HUD 활성화
        hud.gameObject.SetActive(true);

        // HUD 정보 업데이트
        hud.UpdateHP(Stat.HP, Stat.MaxHP);
        hud.UpdateSP(Stat.SP, Stat.MaxSP);
    }

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    protected override void SelectAction()
    {
        StartCoroutine(SelectMotion());
    }

    private IEnumerator SelectMotion()
    {
        // 해당 턴인 캐릭터를 싱글샷
        BattleCameraDirector.Instance.FocusSingle(gameObject);

        // 행동 선택창을 열기 위한 카메라 이동
        yield return BattleCameraDirector.Instance.DirectSelectMotion();

        // 행동 선택창 열기
        actionSelection.OnSelect(this);
    }

    public override void Run()
    {
        List<Character> partyList = battleData.CharacterList;

        // 플레이어의 파티 모두 같이 도주
        for (int i = partyList.Count - 1; i >= 0; i--)
        {
            // 리스트에 오류가 생기지 않도록 역순으로 파괴(도주)
            partyList[i].RunBattle();
        }
    }

    private void RunBattle()
    {
        base.Run();
    }

    public override void UseItem(Consumable item, List<Entity> targets)
    {
        // 아이템 사용
        base.UseItem(item, targets);

        // 플레이어블 캐릭터인 경우 인벤토리에 있는 아이템의 개수 차감
        InventoryData.Instance.RemoveItem(item);
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public override void OnDead()
    {
        // 기존 사망 처리 실행
        base.OnDead();

        // 사망 모션
    }

    protected override void OnParryAction()
    {
        battleData.IsUsedParry = true;
    }

    protected override void OnDodgeAction()
    {
        battleData.IsUsedDodge = true;
    }

    public override void OnParrying(Entity attacker)
    {
        // 패링 성공 알림
        GameEventResource.Instance.ParryingEvent.NotifyUpdate();

        // 패링 모션 실행
        motionManager.ActMotion("parry");

        // 플레이어가 패링에 성공했을 경우 추가타를 넣을 대상 선택
        assistManager.OnSelectExtraAttacker(attacker, this);
    }
}