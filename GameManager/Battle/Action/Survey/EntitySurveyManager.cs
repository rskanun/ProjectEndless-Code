using UnityEngine;

public class EntitySurveyManager : MonoBehaviour
{

    [Header("참조 스크립트")]
    [SerializeField] private SurveyHUD surveyHUD;
    [SerializeField] private SurveyEffectManager effectManager;
    [SerializeField] private ActionIcon actionIcon;

    /***************************************************************
    * [ 상태 관찰 ]
    * 
    * 해당 오브젝트의 상태 관찰에 따른 ui 변화
    ***************************************************************/

    public void ActiveActionIcon(ActionType type)
    {
        actionIcon.SetIcon(type);
    }

    public void HideActionIcon()
    {
        actionIcon.ClearIcon();
    }

    public void SetForecastHP(int hp, int maxHP, int change)
    {
        surveyHUD.SetForecastHP(hp, maxHP, change);
    }

    public void SetActiveForecastHP(bool isActive)
    {
        surveyHUD.SetHpBarActive(isActive);
    }

    public void SetForecastEffect(StatusEffect effect)
    {
        effectManager.CreateForecastEffect(effect);
    }

    public void ClearForecastEffect()
    {
        effectManager.ClearForecastEffect();
    }
}