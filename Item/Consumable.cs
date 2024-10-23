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
    [SerializeReference]
    [ContextMenuItem("Buff", "SetBuff")]
    [ContextMenuItem("Debuff", "SetDebuff")]
    private StatusEffect _effect = new Buff();
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

    public void SetBuff()
    {
        _effect = new Buff();
    }

    public void SetDebuff()
    {
        _effect = new Debuff();
    }
}