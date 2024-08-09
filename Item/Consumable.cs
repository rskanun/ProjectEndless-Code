using UnityEngine;

public class Consumable : Item
{
    private TargetType _targetType;
    public TargetType TargetType
    {
        get { return _targetType; }
    }

    public void OnUse(Entity target)
    {

    }
}