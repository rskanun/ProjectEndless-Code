using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;




#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Motion
{
    public string name;
    public AnimationClip clip;
}

[RequireComponent(typeof(Entity))]
public class EntityMotionManager : MonoBehaviour
{
    [SerializeField]
    private Entity entity;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private bool lookAtRight;

    [Header("근접 공격 및 스킬")]
    [SerializeField]
    private List<Motion> meleeMotions;
    [SerializeField, HideInInspector]
    private List<MeleeRangeEntry> meleeRanges; // dict 저장용
    private Dictionary<string, float> meleeRangeDict;

    private bool _isActing;
    public bool IsActing
    {
        private set { _isActing = value; }
        get { return _isActing; }
    }
    private bool _isIdle = true;
    public bool IsIdle
    {
        private set { _isIdle = value; }
        get { return _isIdle; }
    }
    private Queue<string> motionQueue = new Queue<string>();

#if UNITY_EDITOR
    [ContextMenu("Reload")]
    private void OnValidate()
    {
        // 엔티티 등록
        entity = GetComponent<Entity>();

        // 근접 공격 및 스킬 범위 설정
        SetMeleeRanges();
    }

    private void SetMeleeRanges()
    {
        meleeRanges = new List<MeleeRangeEntry>();

        foreach (Motion motion in meleeMotions)
        {
            if (string.IsNullOrEmpty(motion.name) || motion.clip == null) continue;

            meleeRanges.Add(new MeleeRangeEntry(motion.name, GetRange(motion.clip)));
        }
    }

    private float GetRange(AnimationClip clip)
    {
        float maxRange = 0.0f;

        var bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

        foreach (var binding in bindings)
        {
            if (binding.type != typeof(SpriteRenderer) || binding.propertyName != "m_Sprite")
            {
                // Sprite 타입이 아닌 경우 넘기기
                continue;
            }

            var keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
            foreach (var keyframe in keyframes)
            {
                if (keyframe.value is not Sprite sprite || sprite == null)
                {
                    // Sprite를 가지고 있지 않은 구간인 경우 넘기기
                    continue;
                }

                // 가장 범위가 긴 sprite 찾기
                // sprite 가로 길이 + 해당 스프라이트의 pivot에서 중심(0.5, 0.5)까지의 거리
                Vector3 spritePivot = sprite.pivot / 100.0f;
                float scale = gameObject.transform.localScale.x;
                float range = (sprite.rect.width / sprite.pixelsPerUnit * scale + spritePivot.x) / 2.0f;

                maxRange = Mathf.Max(maxRange, range);
            }
        }

        return maxRange;
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeRangeDict == null || meleeRangeDict.Count <= 0) return;

        OnDrawMeleeRanges(meleeRangeDict.Values.ToList());
    }

    private void OnDrawMeleeRanges(List<float> meleeRange)
    {
        if (spriteRenderer == null || meleeRange == null || meleeRange.Count <= 0) return;

        float vectorX = spriteRenderer.flipX ? -1 : 1;

        Gizmos.color = Color.red;
        foreach (float range in meleeRange)
        {
            Vector2 offset = new Vector2(transform.position.x - range * vectorX, transform.position.y);
            Gizmos.DrawLine(transform.position, offset);
        }
    }
#endif

    private void Awake()
    {
        // 저장용 리스트를 Dict로 옮기기
        meleeRangeDict = meleeRanges.ToDictionary(e => e.name, e => e.range);
    }

    private Vector2 GetMovePoint(Vector2 target, string motion)
    {
        float vectorX = spriteRenderer.flipX != lookAtRight ? -1 : 1;
        return new Vector2(target.x + meleeRangeDict[motion] * vectorX, target.y);
    }

    private bool IsPlayAnimation(string motion = "Idle")
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(motion);
    }

    /***************************************************************
    * [ 모션 ]
    * 
    * 오브젝트의 한 동작 실행 관리
    ***************************************************************/

    public void ActMotion(string motion)
    {
        IsActing = true;
        motionQueue.Enqueue(motion);

        animator.SetTrigger(motion);
    }

    public void OnEndMotion()
    {
        IsActing = false;
        motionQueue.Dequeue();

        // 아직 남은 모션이 실행 중이라면
        if (motionQueue.Count > 0)
        {
            // 다시 모션 실행 중으로 변경
            IsActing = true;
        }
    }

    /***************************************************************
    * [ 애니메이션 ]
    * 
    * 오브젝트의 연속적인 동작 실행 관리
    ***************************************************************/

    private void ActAnimation(IEnumerator actionAnimation)
    {
        // 각 행동을 실행
        IsIdle = false;

        // 행동 모션 체크
        StartCoroutine(PlayAnimation(actionAnimation));
    }

    private IEnumerator PlayAnimation(IEnumerator actionAnimation)
    {
        // 행동 모션
        yield return StartCoroutine(actionAnimation);

        // 행동 모션이 끝났음을 알림
        IsIdle = true;
    }

    /***************************************************************
    * [ 공격 애니메이션 ]
    * 
    * 시전 모션 -> 이동 모션 -> 타격 모션 -> 복귀 모션
    ***************************************************************/

    public void ActAttackAnimation(Entity target, Action onAttack)
    {
        ActAnimation(AttackAnimation(target, onAttack));
    }

    private IEnumerator AttackAnimation(Entity target, Action onAttack)
    {
        Vector2 originPos = transform.position;

        // 공격하는 엔티티를 향해 카메라 포커싱
        BattleCameraDirector.Instance.FocusSingle(gameObject);

        // 모션 실행
        ActMotion("atk");

        // 시전 모션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsPlayAnimation("Attack_Ready"));
        yield return new WaitWhile(() => IsPlayAnimation("Attack_Ready"));

        // 타겟을 향해 카메라 포커싱
        StartCoroutine(BattleCameraDirector.Instance.DirectSmoothFocusing(target.gameObject));

        // 타겟 앞으로 이동
        transform.position = GetMovePoint(target.transform.position, "attack");

        // 모션 체크
        while (IsActing)
        {
            // 공격 모션 중간 패링을 당했을 경우
            if (entity.HasState(EntityState.Stagger))
            {
                // 패링 애니메이션 실행 및 공격 애니메이션 종료
                StartCoroutine(CounterattackAnimation(originPos));
                yield break;
            }

            yield return null;
        }

        // 공격 모션이 끝까지 진행되었을 경우 공격 실행
        onAttack.Invoke();

        // 원래 자리로 돌아오기
        StartCoroutine(ReturnAnimation(originPos));

        // 히트 & 사망 모션 대기
        yield return new WaitUntil(() => target.IsIdle);
    }

    private IEnumerator CounterattackAnimation(Vector2 originPos)
    {
        // 패링 당한 엔티티(=공격을 감행한 엔티티)를 향해 카메라 포커싱
        BattleCameraDirector.Instance.FocusSingle(gameObject);

        // 반격(패링 당하는) 모션 실행
        ActMotion("counter");

        // 히트 모션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsPlayAnimation("Hit"));
        yield return new WaitWhile(() => IsActing);

        // 사망 상태가 아니라면 원래 자리로 복귀하는 모션 실행
        if (!entity.IsDead) ActMotion("return");

        // 모션이 끝날 때까지 기다리기
        yield return new WaitWhile(() => IsActing);

        // 모션이 끝나면 원래 자리로 복귀
        transform.position = originPos;
    }

    /***************************************************************
    * [ 히트 애니메이션 ]
    * 
    * 히트 모션 실행, 이후 사망했다면 사망 모션까지 실행
    ***************************************************************/

    public void ActHitAnimation()
    {
        ActAnimation(HitAnimation());
    }

    private IEnumerator HitAnimation()
    {
        // 히트 모션 실행
        ActMotion("hit");

        // 사망 체크
        while (IsActing)
        {
            // 사망한 경우 해당 애니메이션 실행
            if (entity.IsDead)
            {
                yield return StartCoroutine(DeadAnimation());
                yield break;
            }

            yield return null;
        }
    }

    protected virtual IEnumerator DeadAnimation()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // 사망 모션 실행
        ActMotion("death");

        // 사망 모션이 끝날 때까지 대기
        yield return new WaitUntil(() => IsPlayAnimation("Death"));
        yield return new WaitWhile(() => IsActing);

        // 페이드 아웃
        DOTween.Sequence()
            .Append(sprite.DOFade(0.0f, 1.5f))
            .OnComplete(() => gameObject.SetActive(false))
            .WaitForCompletion();
    }

    /***************************************************************
    * [ 복귀 애니메이션 ]
    * 
    * 근접 공격 실행 후, 본래 자리로 돌아오는 애니메이션
    ***************************************************************/

    private IEnumerator ReturnAnimation(Vector2 originPos)
    {
        // 복귀 모션 실행
        ActMotion("return");

        // 모션이 끝날 때까지 대기
        yield return new WaitWhile(() => IsActing);

        // 모션이 끝나면 원래 자리로 복귀
        transform.position = originPos;
    }

    [System.Serializable]
    private class MeleeRangeEntry
    {
        public string name;
        public float range;

        public MeleeRangeEntry(string name, float range)
        {
            this.name = name;
            this.range = range;
        }
    }
}