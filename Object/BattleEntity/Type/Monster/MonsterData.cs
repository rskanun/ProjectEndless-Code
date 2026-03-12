using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    [Range(0, 100)]
    public int dropChance;
    public int maxCount;
    public Item dropItem;
}

[CreateAssetMenu(menuName = "Entity Data/Monster", fileName = "Monster Data")]
public class MonsterData : EntityData
{
    [Title("획득 보상")]
    [SerializeField, PropertyOrder(50)]
    private int _minAmount;
    public int MinAmount => _minAmount;
    [SerializeField, PropertyOrder(50)]
    private int _maxAmount;
    public int MaxAmount => _maxAmount;

    [SerializeField, PropertyOrder(50)]
    private List<DropItem> _dropItems;
    public List<DropItem> DropItems => _dropItems;
}