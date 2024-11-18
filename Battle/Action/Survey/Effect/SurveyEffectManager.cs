using System.Collections.Generic;
using UnityEngine;

public class SurveyEffectManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SurveyEffectUI ui;
    [SerializeField] private StatusEffectUI effectUI;
    [SerializeField] private StatusEffectManager effectManager;

    // 예상 상태효과 목록
    private List<StatusEffect> forecastEffects;

    private void Awake()
    {
        forecastEffects = new List<StatusEffect>();
    }

    public void CreateForecastEffect(StatusEffect effect)
    {
        // 예약 아이콘 생성
        if (effect.IsBuff) ui.CreateTempBuffIcon(effect);
        else ui.CreateDebuffIcon(effect);

        // 만약 현재 존재하는 아이콘일 경우 해당 아이콘을 잠시 숨김
        if (effectManager.HasEffect(effect))
        {
            effectUI.HideIcon(effect);
        }

        // 예상 상태효과 목록에 추가
        forecastEffects.Add(effect);
    }

    public void ClearForecastEffect()
    {
        foreach (StatusEffect effect in forecastEffects)
        {
            // 임시로 숨긴 아이콘일 경우 해당 아이콘 다시 활성화
            if (effectManager.HasEffect(effect))
            {
                Debug.Log("Has Effect");
                effectUI.ViewIcon(effect);
            }
        }

        // 생성된 임시 아이콘 삭제
        ui.ClearIcons();

        // 예상 상태효과 목록 초기화
        forecastEffects.Clear();
    }
}