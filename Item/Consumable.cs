using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable", fileName = "Consumable Item")]
public class Consumable : Item
{
    [Header("소비 아이템 정보")]
    [SerializeField]
    private TargetType _targetType;
    public TargetType TargetType
    {
        get { return _targetType; }
    }
    [SerializeField]
    private StatusEffect _effect;
    public StatusEffect Effect
    {
        get { return _effect; }
    }

    public void OnUse(List<Entity> targets)
    {
        foreach (Entity target in targets)
        {
            target.AddEffect(Effect);
        }
    }
}