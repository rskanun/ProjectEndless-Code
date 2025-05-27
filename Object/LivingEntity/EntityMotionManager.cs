using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [Header("근접 공격 및 스킬")]
    [SerializeField]
    private List<Motion> meleeMotions;
    private Dictionary<string, float> meleeRange;

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
                float range = (sprite.rect.width / sprite.pixelsPerUnit + spritePivot.x - 0.5f) * scale;

                maxRange = Mathf.Max(maxRange, range);
            }
        }

        return maxRange;
    }
#endif

    private void OnDrawMeleeRanges(List<(Vector2 size, Vector3 offset)> meleeRange)
    {
        if (meleeRange == null || meleeRange.Count <= 0) return;

        Gizmos.color = Color.red;
        foreach ((Vector2 size, Vector3 offset) in meleeRange)
        {
            float offsetX = offset.x * gameObject.transform.localScale.x;
            float offsetY = offset.y - 0.5f;
            Gizmos.DrawWireCube(transform.position - new Vector3(offsetX, offsetY), size * gameObject.transform.localScale);
        }
    }

}