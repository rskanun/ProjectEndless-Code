using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager
{
    // 현재 지니고 있는 상태효과 목록
    private Dictionary<StatusEffect, float> effects;

    public StatusEffectManager()
    {
        effects = new Dictionary<StatusEffect, float>();
    }

    public void AddEffect(StatusEffect effect)
    {
        // 이미 해당 상태효과이 걸려있는 경우
        if (HasEffect(effect))
        {
            // 해당 상태효과 지속시간 갱신
            effects[effect] = effect.Duration;
        }
        else
        {
            // 상태효과 추가
            effects.Add(effect, effect.Duration);
        }
    }

    public bool HasEffect(StatusEffect effect)
    {
        return effects.ContainsKey(effect);
    }

    public void UpdateEffectTimer(float turn)
    {
        // 지속턴이 지난 상태효과 제거 목록
        List<StatusEffect> removeList = new List<StatusEffect>();

        // 지속턴 업데이트 및 지속턴이 지난 상태효과 찾기
        foreach (StatusEffect key in effects.Keys)
        {
            // 지속턴 감소
            effects[key] -= turn;

            // 지속턴이 지났을 경우 삭제
            if (effects[key] <= 0)
            {
                // 임시목록에 추가
                removeList.Add(key);
            }
        }

        // 상태효과 목록에서 지우기
        foreach (StatusEffect removeEffect in removeList)
        {
            effects.Remove(removeEffect);
        }
    }
}