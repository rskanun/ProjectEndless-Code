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

    // 전투 데이터
    private BattleData battleData;

    private void Awake()
    {
        battleData = BattleData.Instance;
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
        if (battleData.IsInBattle == false)
        {
            // 전투가 끝났을 경우 행동을 하지 않고 종료
            EndTurn();

            return;
        }

        // AI에 따른 행동 처리
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

        // 턴 종료
        EndTurn();
    }

    public override void OnAttack(Entity target)
    {
        throw new System.NotImplementedException();
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
        // 현 전투에서 적 데이터 삭제 및 처지 보상 업데이트
        battleData.AddKillReward(this);
        battleData.RemoveEnemyData(this);

        // 기존 사망 처리 실행
        base.OnDead();

        // 사망 모션
        gameObject.SetActive(false);
    }

    public override void OnManaShort()
    {
        throw new System.NotImplementedException();
    }
}