using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [Header("스킬 정보")]
    [SerializeField] 
    private float _consumeTurn;
    public float ConsumeTurn
    {
        get { return _consumeTurn; }
    }
    [SerializeField]
    private TargetType _targetType;
    public TargetType TargetType
    {
        get { return _targetType; }
    }

    public abstract void OnCasting(Entity target);
}