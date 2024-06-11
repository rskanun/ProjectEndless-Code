using UnityEngine;

// 몬스터 AI = 성향 + 탐지 기관 + 성격 + 개체 특성

// 성향: 플레이어와 대면 했을 때의 행동
public enum EPropensity
{
    Friendly,   // 플레이어에게 이로운 영향을 주지만, 피해를 입을 시 적대적으로 변함
    Neutral,    // 플레이어에게 아무런 영향을 끼치지 않으나, 피해를 입을 시 적대적으로 변함
    Hostile     // 플레이어에게 해로운 영향을 끼침
}
// 성격: 적대적일 때 취하는 행동
public enum EPersonality
{
    Bravery,    // 자신의 체력에 상관없이 공격만을 행함
    Prudence,   // 자신의 체력에 따라 공격을 행하거나 수비적인 태세를 취함
    Skittish    // 플레이어로부터 일정 거리 이상까지 도망침
}
// 개체 특성: 다른 개체들과의 행동
public enum Quality
{
    Independent,    // 같은 개체에 영향을 받지 않음
    Social,         // 동일한 개체끼리 다니며, 한 마리라도 적대적으로 변하면 주변의 동일한 개체들도 적대적으로 변함
    Protective      // 적대적으로 변할 시 주변에 있는 모든 개체들도 적대적으로 변함
}

[CreateAssetMenu(menuName = "Game Object/Monster/Monster", fileName = "Monster")]
public class MonsterData : EntityData
{
    [Header("몬스터 행동 변수")]
    [SerializeField]
    private float _attackDistance;
    public float AttackDistance { get { return _attackDistance; } }
    [SerializeField]
    private float _attackCooldown;
    public float AttackCooldown { get { return _attackCooldown; } }
}