using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    // 참조 스크립트
    [SerializeField] private ActionManager actionSelection;

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
        SkillList = data.Skills;
        OriginStat = data.Stat;

        // 최종스텟 설정
        InitLastStat();

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

    public override void TakeTurn()
    {
        if (battleData.IsInBattle == false)
        {
            // 전투가 끝났을 경우 행동을 하지 않고 종료
            EndTurn();

            return;
        }

        // 행동 선택창 열기
        actionSelection.OnSelect(this);
    }

    protected override Entity GetRetarget(Entity target)
    {
        // 캐릭터의 성격마다 우선순위로 선택하는 타겟이 다름
        return null;
    }

    public override void OnRun()
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
        base.OnRun();
    }

    public void OnSelectAction(BattleAction action, int index)
    {
        // 행동 예약
        battleData.Sequence.AddTurn(action, index);

        // 턴 종료
        EndTurn();
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
}