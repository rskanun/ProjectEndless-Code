using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    [Range(0, 100)]
    public int dropChance;
    public int maxCount;
    public Item dropItem;
}
public class Monster : Entity
{
    [Header("획득 보상")]
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    [SerializeField]
    private List<DropItem> _dropItems;
    public List<DropItem> DropItems
    {
        get { return _dropItems; }
    }

    protected override void Awake()
    {
        base.Awake();

        // 최종스텟 설정
        InitLastStat();
    }

    public int GetDropGold()
    {
        int dropGold = Random.Range(minAmount, maxAmount + 1);

        return dropGold;
    }

    public Dictionary<Item, int> GetDropItems()
    {
        Dictionary<Item, int> result = new Dictionary<Item, int>();

        foreach (DropItem dropItem in DropItems)
        {
            int chance = Random.Range(0, 100) + 1;

            if (chance <= dropItem.dropChance)
            {
                Item item = dropItem.dropItem;
                int count = Random.Range(1, dropItem.maxCount + 1);

                result[item] = count;
            }
        }

        return result;
    }

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public override void TakeTurn()
    {
        if (battleData.IsInBattle)
        {
            // AI에 따른 행동 처리
            SelectAction();
        }
        else
        {
            // 전투가 끝났을 경우 행동을 하지 않고 종료
            EndTurn();
        }
    }

    private void SelectAction()
    {
        // 임시로 상시 대기 실행
        Invoke(nameof(OnWaitingAction), 2.0f);
    }

    private void OnWaitingAction()
    {
        // 임시 대기
        WaitAction waitAction = new WaitAction();

        waitAction.remainTurn = 10.0f;
        waitAction.actor = this;

        battleData.Sequence.AddTurn(waitAction);
        Debug.Log($"{Name}: {waitAction.remainTurn} Turn Waiting...");

        // 턴 종료
        EndTurn();
    }

    protected override Entity OnRetarget(Entity target)
    {
        List<Character> targets = GetTargets();

        // 일반 몬스터는 다음 타겟을 공격대상으로 지정
        int stopIndex = targets.IndexOf((Character)target);
        int curIndex = stopIndex;

        Entity retarget = null;
        while (target != retarget)
        {
            // 공격대상으로 지정 가능한 타겟이 나올 때가지 반복
            curIndex = (curIndex + 1) % targets.Count;
            retarget = targets[curIndex].GetComponent<Character>();

            if (retarget.IsDead == false)
            {
                // 공격 가능한 다음 대상 리턴
                return retarget;
            }
        }

        // 다음 대상이 이전 대상과 동일한 경우 null 리턴
        return null;
    }

    private List<Character> GetTargets()
    {
        // 근접일 경우 전방 캐릭터 목록만 리턴
        if (AttackType == AttackType.Melee) 
            return battleData.CharacterFrontList;

        // 원거리일 경우 모든 캐릭터 목록 리턴
        return battleData.CharacterList;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 오브젝트의 이벤트에 의한 상태 처리
    ***************************************************************/

    public override void OnDamage(float damage, int targetMP)
    {
        int curHP = Stat.HP;
        base.OnDamage(damage, targetMP);
        Debug.Log($"{Name} {curHP - Stat.HP} Damage!!");
    }

    public override void OnDead()
    {
        // 기존 사망 처리 실행
        base.OnDead();

        // 처지 보상 업데이트
        battleData.AddKillReward(this);

        // 사망 모션
    }
}