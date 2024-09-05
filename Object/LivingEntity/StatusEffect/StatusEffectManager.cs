using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private class ActiveEffect
    {
        public StatusEffect effect;
        public float remainDuration;
        public Action endAction;

        public ActiveEffect(StatusEffect effect, Action endAction)
        {
            this.effect = effect;
            this.endAction = endAction;

            remainDuration = effect.Duration;
        }
    }

    [SerializeField]
    private StatusEffectUI ui;

    // 지니고 있는 상태효과 목록
    private Dictionary<StatusEffect, ActiveEffect> activeEffectList;

    // 예상 상태효과 목록
    private List<StatusEffect> forecastEffects;

    private void Awake()
    {
        activeEffectList = new Dictionary<StatusEffect, ActiveEffect>();
        forecastEffects = new List<StatusEffect>();
    }

    public void AddEffect(StatusEffect effect, Action startAction, Action endAction)
    {
        // 이미 해당 상태효과이 걸려있는 경우
        if (HasEffect(effect))
        {
            // 해당 상태효과 지속시간 갱신
            activeEffectList[effect].remainDuration = effect.Duration;
        }
        else
        {
            ActiveEffect activeEffect = new ActiveEffect(effect, endAction);

            // 상태효과 추가
            activeEffectList.Add(effect, activeEffect);

            // 아이콘 추가
            AddEffectIcon(effect);

            // 효과 주기
            startAction?.Invoke();
        }
    }

    private void AddEffectIcon(StatusEffect effect)
    {
        if (effect.IsBuff) ui.CreateBuffIcon(effect);
        else ui.CreateDebuffIcon(effect);
    }

    public bool HasEffect(StatusEffect effect)
    {
        return activeEffectList.ContainsKey(effect);
    }

    public void UpdateEffectTimer(float turn)
    {
        // 지속턴이 지난 상태효과 제거 목록
        List<ActiveEffect> removeList = new List<ActiveEffect>();

        // 지속턴 업데이트 및 지속턴이 지난 상태효과 찾기
        foreach (ActiveEffect activeEffect in activeEffectList.Values)
        {
            // 지속턴 감소
            activeEffect.remainDuration -= turn;

            // 지속턴이 지났을 경우 삭제
            if (activeEffect.remainDuration <= 0)
            {
                // 임시목록에 추가
                removeList.Add(activeEffect);
            }
        }

        // 상태효과 목록에서 지우기
        foreach (ActiveEffect removeEffect in removeList)
        {
            // 효과 지우기
            removeEffect.endAction?.Invoke();

            // 목록에서 삭제
            activeEffectList.Remove(removeEffect.effect);
        }
    }

    public void CreateForecastEffect(StatusEffect effect)
    {
        // 예약 아이콘 생성
        if (effect.IsBuff) ui.CreateTempBuff(effect);
        else ui.CreateTempDebuff(effect);

        // 만약 현재 존재하는 아이콘일 경우 해당 아이콘을 잠시 숨김
        if (HasEffect(effect))
        {
            ui.HideIcon(effect);
        }

        // 예상 상태효과 목록에 추가
        forecastEffects.Add(effect);
    }

    public void ClearForecastEffect()
    {
        foreach (StatusEffect effect in forecastEffects)
        {
            // 임시로 숨긴 아이콘일 경우 해당 아이콘 다시 활성화
            if (HasEffect(effect))
            {
                Debug.Log("Has Effect");
                ui.ViewIcon(effect);
            }
        }

        // 생성된 임시 아이콘 삭제
        ui.ClearTempIcons();
        
        // 예상 상태효과 목록 초기화
        forecastEffects.Clear();
    }
}