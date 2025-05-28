using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Motion
{
    public string name;
    public AnimationClip clip;
}

public class EntityMotionManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

    [Header("근접 공격 및 스킬")]
    [SerializeField]
    private List<Motion> meleeMotions;
    private Dictionary<string, float> meleeRange;

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
    private string _motion;
    public string Motion
    {
        private set { _motion = value; }
        get { return _motion; }
    }

    private Coroutine motionAnimation;

#if UNITY_EDITOR
    [ContextMenu("Reload")]
    private void OnValidate()
    {
        meleeRange = new Dictionary<string, float>();

        foreach (Motion motion in meleeMotions)
        {
            if (string.IsNullOrEmpty(motion.name) || motion.clip == null) continue;

            meleeRange.Add(motion.name, GetRange(motion.clip));
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
        OnDrawBody();
        OnDrawMeleeRanges(meleeRange.Values.ToList());
    }

    private void OnDrawBody()
    {
        Vector3 offset = transform.TransformPoint(spriteRenderer.sprite.bounds.center);
        Vector3 size = Vector3.Scale(spriteRenderer.sprite.bounds.size, transform.lossyScale);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(offset, size);
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


    // x 좌표 => (어태커 위치 + 공격 범위) - (타겟 위치 + 피격 범위) < 오차 범위
    // y 좌표 => 어태커 위치 == 타겟 위치
    // 이동 방법 => lerp?

    // 공격 방식
    // 시작 모션 -> 이동 -> 공격 모션

    /***************************************************************
    * [ 모션 ]
    * 
    * 오브젝트의 한 동작 실행 관리
    ***************************************************************/

    public void ActMotion(string motion)
    {
        IsActing = true;
        Motion = motion;

        animator.SetTrigger(motion);
    }

    public void OnEndMotion()
    {
        IsActing = false;
        Motion = "idle";
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
        motionAnimation = StartCoroutine(PlayAnimation(actionAnimation));
    }

    private IEnumerator PlayAnimation(IEnumerator actionAnimation)
    {
        // 행동 모션
        yield return StartCoroutine(actionAnimation);

        // 행동 모션이 끝났음을 알림
        motionAnimation = null;
        IsIdle = true;
    }

    public void StopAnimation()
    {
        if (motionAnimation == null) return;

        // 진행 중이던 애니메이션 강제 종료
        StopCoroutine(motionAnimation);
        IsIdle = false;
    }

    /***************************************************************
    * [ 공격 애니메이션 ]
    * 
    * 시전 모션 -> 이동 -> 타격 모션
    ***************************************************************/

    public void ActAttackAnimation()
    {
        PlayAnimation(AttackAnimation());
    }

    private IEnumerator AttackAnimation()
    {

    }
}