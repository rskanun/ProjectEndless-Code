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

    private float AttackTurn => GetLastTurn(1.0f);

    protected override void Awake()
    {
        base.Awake();

        // 최종스텟 설정
        InitLastStat();

        // HUD 업데이트
        InitHUD();
    }

    private void InitHUD()
    {
        // HUD 업데이트
        hud.UpdateHP(Stat.HP, Stat.MaxHP);
        hud.UpdateMP(Stat.MP, Stat.MaxMP);
        hud.UpdateSP(Stat.SP, Stat.MaxSP);
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

    protected virtual void SelectAction()
    {
        // 임시로 상시 대기 실행
        Invoke(nameof(SelectWait), 2.0f);
    }

    protected void SelectSkill(Skill skill, Entity target, int? index = null)
    {
        List<Entity> targetList = new List<Entity>() { target };
        SelectSkill(skill, targetList, index);
    }

    protected virtual void SelectSkill(Skill skill, List<Entity> targets, int? index = null)
    {
        Debug.Log($"{Name}: Select Skill");
        SkillAction action = new SkillAction();

        action.actor = this;
        action.castSkill = skill;
        action.remainTurn = skill.CostTurn;
        action.SetTarget(targets);

        OnSelectAction(action, index);
    }

    protected virtual void SelectAttack(Entity target, int? index = null)
    {
        Debug.Log($"{Name}: Select Attack to {target.Name}");
        AttackAction action = new AttackAction();

        action.actor = this;
        action.target = target;
        action.remainTurn = AttackTurn;

        OnSelectAction(action);
    }

    private void SelectWait()
    {
        // 임시 대기
        WaitAction action = new WaitAction();

        action.remainTurn = 10.0f;
        action.actor = this;

        Debug.Log($"{Name}: {action.remainTurn} Turn Waiting...");
        OnSelectAction(action);
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

        // 처지 보상 업데이트
        battleData.AddKillReward(this);

        // 사망 모션
    }
}