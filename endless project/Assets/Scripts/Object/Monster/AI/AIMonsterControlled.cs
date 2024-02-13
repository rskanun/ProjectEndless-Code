using UnityEngine;

// 성향: 플레이어와 대면 했을 때의 행동
public enum Propensity
{
    Friendly,   // 플레이어에게 이로운 영향을 주지만, 피해를 입을 시 적대적으로 변함
    Neutral,    // 플레이어에게 아무런 영향을 끼치지 않으나, 피해를 입을 시 적대적으로 변함
    Hostile     // 플레이어에게 해로운 영향을 끼침
}
// 적대적일 때 취하는 행동
public enum Personality
{
    Bravery,    // 자신의 체력에 상관없이 공격만을 행함
    Prudence,   // 자신의 체력에 따라 공격을 행하거나 수비적인 태세를 취함
    Skittish    // 플레이어로부터 일정 거리 이상까지 도망침
}
// 다른 개체들과의 행동
public enum Quality
{
    Independent,    // 같은 개체에 영향을 받지 않음
    Social,         // 동일한 개체끼리 다니며, 한 마리라도 적대적으로 변하면 주변의 동일한 개체들도 적대적으로 변함
    Protective      // 적대적으로 변할 시 주변에 있는 모든 개체들도 적대적으로 변함
}
public class AIMonsterControlled : MonoBehaviour
{
    // 몬스터 AI
    [SerializeField] private Personality personality;
    [SerializeField] private Quality quality;

    // 몬스터 성향
    [SerializeField]
    private Propensity propensity; // 기본 성향
    private Propensity currentPropensity; // 현재 성향

    // 몬스터 이동 관련 변수
    [SerializeField] private bool isMoving;
    [SerializeField] private float moveRadius;
    [SerializeField] private Vector2 targetPos; // 이동할 최종 목적지

    // 플레이어 탐지 반경
    [SerializeField] private Vector2 detectionArea;

    // 참조 스크립트
    private Monster monster;

    private void Awake()
    {
        monster = gameObject.GetComponent<Monster>();
    }

    private void OnEnable()
    {
        currentPropensity = propensity;
    }

    public void OnTakeDamage()
    {
        if (currentPropensity != Propensity.Hostile)
        {
            // 공격을 당할 시, 적대적인 성향을 띔
            currentPropensity = Propensity.Hostile;
        }
    }

    /***************************************************************
    * [ 기본(Idle) 상태 ]
    * 
    * 자유롭게 행동하는 상태
    ***************************************************************/

    public void OnIdleAction()
    {
        if (isMoving)
        {
            // targetPos 까지 움직임
            // 플레이어 탐지
        }
        else
        {
            // 행동(움직임 or 가만히) 결정
        }
    }

    private void ThinkIdleAction()
    {

    }
}