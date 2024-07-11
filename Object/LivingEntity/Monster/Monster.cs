using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    [Range(0, 100)]
    public int dropChance;
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

    // 전투 순서 데이터
    private BattleSequence battleSeq;

    private void Awake()
    {
        battleSeq = BattleData.Instance.Sequence;
    }

    public int GetDropGold()
    {
        int dropGold = Random.Range(minAmount, maxAmount + 1);

        return dropGold;
    }

    public List<Item> GetDropItems()
    {
        List<Item> dropItems = new List<Item>();

        foreach (DropItem item in DropItems)
        {
            int chance = Random.Range(0, 100) + 1;

            if (chance <= item.dropChance)
            {
                dropItems.Add(item.dropItem);
            }
        }

        return dropItems;
    }

    /***************************************************************
    * [ 턴 진행 ]
    * 
    * 해당 오브젝트의 턴 진행
    ***************************************************************/

    public override void TakeTurn()
    {
        // AI에 따른 행동 처리
        // 임시로 상시 대기 실행
        Invoke(nameof(OnWaitingAction), 2.0f);
    }

    public override void OnAttack(Entity target)
    {
        throw new System.NotImplementedException();
    }

    public void OnWaitingAction()
    {
        WaitAction action = new WaitAction();

        action.actor = this;
        action.remainTurn = 5.0f / Stat.AGI;

        Debug.Log($"{Name} {action.remainTurn} 턴 뒤, 대기 행동 예약");
        battleSeq.AddTurn(action);

        // 행동 종료
        EndTurn();
    }

    public override void OnWaiting()
    {
        Debug.Log($"{Name} 대기");
        base.OnWaiting();
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
        BattleData.Instance.KilledEnemy(this);

        // 오브젝트 삭제
        Destroy(gameObject);
    }

    public override void OnManaShort()
    {
        throw new System.NotImplementedException();
    }
}