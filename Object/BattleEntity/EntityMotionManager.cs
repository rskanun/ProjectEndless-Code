using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class AnimParams
{
    // 성능 최적화를 위한 문자열을 정수로 해싱
    // Triggerr
    public static readonly int AttackTrigger = Animator.StringToHash("attack");
    public static readonly int CounterTrigger = Animator.StringToHash("counter");
    public static readonly int CounterattackTrigger = Animator.StringToHash("counterattack");
    public static readonly int DodgeTrigger = Animator.StringToHash("dodge");
    public static readonly int ParryTrigger = Animator.StringToHash("parry");
    public static readonly int ReturnTrigger = Animator.StringToHash("return");
    public static readonly int DeathTrigger = Animator.StringToHash("death");
    public static readonly int HitTrigger = Animator.StringToHash("hit");

    // Motion Name
    public static readonly int IdleMotion = Animator.StringToHash("Idle");
    public static readonly int AttackReadyMotion = Animator.StringToHash("Attack_Ready");
    public static readonly int AttackMotion = Animator.StringToHash("Attack");
    public static readonly int CounterattackMotion = Animator.StringToHash("Counterattack");
    public static readonly int DeathMotion = Animator.StringToHash("Death");
    public static readonly int HitMotion = Animator.StringToHash("Hit");
}

[Serializable]
public class Motion
{
    public string name;
    public AnimationClip clip;
}

[RequireComponent(typeof(Entity))]
public class EntityMotionManager : MonoBehaviour
{
    #region [Settings & References]
    [SerializeField, ReadOnly] private Entity entity;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private bool lookAtRight;
    [SerializeField][Range(-1.0f, 1.0f)] private float range;

    [Title("근접 공격 및 스킬")]
    [SerializeField] private List<Motion> meleeMotions;
    [SerializeField] private List<MeleeRangeEntry> meleeRanges; // dict 저장용
    private Dictionary<int, float> meleeRangeLookup;
    #endregion

    #region [State References]
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
    private Action onHitAction;
    private Queue<int> motionQueue = new Queue<int>();
    #endregion

    [Serializable]
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

    private void Awake()
    {
        // 저장용 리스트를 Dictionary로 옮기기
        meleeRangeLookup = meleeRanges.ToDictionary(e => Animator.StringToHash(e.name), e => e.range);
    }

    private Vector2 GetMovePoint(Vector2 target, float motionRange)
    {
        float vectorX = spriteRenderer.flipX != lookAtRight ? -1f : 1f;
        return new Vector2(target.x + (motionRange - range) * vectorX, target.y);
    }

    private bool IsPlayAnimation(int stateHash)
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == stateHash;
    }

    /***************************************************************
    * [ 모션 ]
    * 
    * 오브젝트의 한 동작 실행 관리
    ***************************************************************/

    public void ActMotion(int id)
    {
        IsActing = true;

        motionQueue.Enqueue(id);
        animator.SetTrigger(id);
    }

    public void OnEndMotion()
    {
        // 종료 시킬 모션이 없으면 그대로 종료
        if (motionQueue.Count <= 0) return;

        IsActing = false;
        motionQueue.Dequeue();

        // 아직 남은 모션이 실행 중이라면
        if (motionQueue.Count > 0)
        {
            // 다시 모션 실행 중으로 변경
            IsActing = true;
        }
    }

    public void OnHit()
    {
        // 타격 이벤트 실행
        onHitAction?.Invoke();
    }

    /***************************************************************
    * [ 공격 애니메이션 ]
    * 
    * 대상을 공격하는 모션을 근거리와 원거리로 나눠 애니메이션 진행
    ***************************************************************/
    public async UniTask ActMeleeAttackAnimation(Entity target, bool isParryable, bool isDodgeable, int animMotionHash, int animTriggerHash, Action onHit)
    {
        IsIdle = false;

        // 본래 위치 기억
        Vector2 originPos = transform.position;

        // 공격하는 엔티티를 향해 카메라 포커싱
        BattleCameraDirector.Instance.FocusSingle(gameObject);

        // 모션 실행
        ActMotion(animTriggerHash);

        // 타격 타이밍에 맞춰 데미지 넣기
        onHitAction += onHit;

        // 타격 모션까지 대기
        await UniTask.WaitUntil(() => IsPlayAnimation(animMotionHash));

        // 대상에게 타겟에 선택되었음을 알림
        target.OnTargetedAttack(entity, isParryable, isDodgeable);

        // 타겟을 향해 카메라 포커싱
        BattleCameraDirector.Instance.DirectSmoothFocusing(target.gameObject).Forget();

        // 타겟 앞으로 이동
        var range = meleeRangeLookup[animTriggerHash];
        transform.position = GetMovePoint(target.transform.position, range);

        // 모션 체크
        while (IsActing)
        {
            // 공격 모션 중간 패링을 당했을 경우
            if (entity.HasState(EntityState.Stagger))
            {
                // 기존 타격 이벤트 삭제
                onHitAction = null;

                // 패링 애니메이션 실행 및 공격 애니메이션 종료
                await ParriedAnimation(target, originPos);
                return;
            }

            // 1프레임씩 확인
            await UniTask.Yield();
        }

        // 원래 자리로 돌아오기
        await ReturnAnimation(originPos);

        // 히트 & 사망 모션 대기
        await UniTask.WaitUntil(() => target.IsIdle);

        // 타격 이벤트 삭제
        onHitAction = null;

        // 행동 모션이 끝났음을 알림
        IsIdle = true;
    }

    private async UniTask ParriedAnimation(Entity target, Vector2 originPos)
    {
        // 이전 공격 모션 종료
        OnEndMotion();

        // 현재 행동의 주체들을 향해 카메라 포커싱
        var group = new List<GameObject> { gameObject, target.gameObject };
        BattleCameraDirector.Instance.FocusGroup(group);

        // 반격(패링) 당하는 모션 실행
        ActMotion(AnimParams.CounterTrigger);

        // 히트 모션이 끝날 때까지 대기
        await UniTask.WaitUntil(() => IsPlayAnimation(AnimParams.HitMotion));
        await UniTask.WaitWhile(() => IsActing);

        // 반격으로 엔티티가 사망했다면
        if (entity.IsDead)
        {
            // 사망 모션 기다리고 종료
            await UniTask.WaitWhile(() => IsActing);
            return;
        }

        // 원래 자리로 복귀하는 모션 실행
        ActMotion(AnimParams.ReturnTrigger);

        // 모션이 끝날 때까지 기다리기
        await UniTask.WaitWhile(() => IsActing);

        // 모션이 끝나면 원래 자리로 복귀
        transform.position = originPos;
    }

    public async UniTask ActRangeAttackAnimation(Entity target, bool isParryable, bool isDodgeable, Action onHit)
    {
        IsIdle = false;

        // 공격하는 엔티티를 향해 카메라 포커싱
        BattleCameraDirector.Instance.FocusSingle(gameObject);

        // 모션 실행
        ActMotion(AnimParams.AttackTrigger);

        // 시전 모션이 끝날 때까지 대기
        await UniTask.WaitUntil(() => IsPlayAnimation(AnimParams.AttackMotion));
        await UniTask.WaitWhile(() => IsPlayAnimation(AnimParams.AttackMotion));

        // 타겟을 향해 카메라 포커싱
        BattleCameraDirector.Instance.DirectSmoothFocusing(target.gameObject).Forget();

        // 원거리 오브젝트 생성

        // 행동 모션이 끝났음을 알림
        IsIdle = true;
    }

    /***************************************************************
    * [ 반격 애니메이션 ]
    * 
    * 패링 성공 시, 일반 반격 공격 모션 실행
    ***************************************************************/

    public async UniTask ActCounterattackAnimation(Action onHit)
    {
        IsIdle = false;

        // 반격 대상 포커싱
        BattleCameraDirector.Instance.FocusSingle(gameObject);

        // 반격 모션 실행
        ActMotion(AnimParams.CounterattackTrigger);

        // 타격 타이밍에 맞춰 데미지 넣기
        onHitAction += onHit;

        // 모션이 끝날 때까지 대기
        await UniTask.WaitUntil(() => IsPlayAnimation(AnimParams.CounterattackMotion));
        await UniTask.WaitWhile(() => IsActing);

        // 타격 이벤트 삭제
        onHitAction = null;

        // 행동 모션이 끝났음을 알림
        IsIdle = true;
    }

    /***************************************************************
    * [ 타격 애니메이션 ]
    * 
    * 타격 모션 실행, 이후 사망했다면 사망 모션까지 실행
    ***************************************************************/

    public async UniTask ActHitAnimation()
    {
        IsIdle = false;

        // 타격 모션 실행
        ActMotion(AnimParams.HitTrigger);

        // 사망 체크
        while (IsActing)
        {
            // 사망한 경우 해당 애니메이션 실행
            if (entity.IsDead)
            {
                await DeadAnimation();

                // 행동 모션이 끝났음을 알림
                IsIdle = true;
                return;
            }

            // 1프레임씩 확인
            await UniTask.Yield();
        }

        // 행동 모션이 끝났음을 알림
        IsIdle = true;
    }

    protected virtual async UniTask DeadAnimation()
    {
        // 사망 모션 실행
        ActMotion(AnimParams.DeathTrigger);

        // 사망 모션이 끝날 때까지 대기
        await UniTask.WaitUntil(() => IsPlayAnimation(AnimParams.DeathMotion));
        await UniTask.WaitWhile(() => IsActing);

        // 페이드 아웃
        await spriteRenderer.DOFade(0.0f, 1.5f).ToUniTask();
        gameObject.SetActive(false);
    }

    /***************************************************************
    * [ 복귀 애니메이션 ]
    * 
    * 근접 공격 실행 후, 본래 자리로 돌아오는 애니메이션
    ***************************************************************/

    private async UniTask ReturnAnimation(Vector2 originPos)
    {
        // 복귀 모션 실행
        ActMotion(AnimParams.ReturnTrigger);

        // 모션이 끝날 때까지 대기
        await UniTask.WaitWhile(() => IsActing);

        // 모션이 끝나면 원래 자리로 복귀
        transform.position = originPos;
    }

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
        if (meleeRangeLookup == null || meleeRangeLookup.Count <= 0) return;

        OnDrawMeleeRanges(meleeRangeLookup.Values.ToList());
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
}