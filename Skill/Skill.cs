using UnityEngine;

public enum TargetType
{
    FrontEnemy, // 적 진형 선열 1명
    Enemy,      // 적 진형 1명
    AllEnemy,   // 모든 적
    Member,     // 파티 맴버 1명
    Party,      // 모든 파티 맴버
    Caster      // 사용자
}
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