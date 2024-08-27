using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIcon : MonoBehaviour
{
    public Image icon;

    // 상태 효과 정보
    private StatusEffect effect;

    public void SetEffect(StatusEffect effect)
    {
        this.effect = effect;

        // 아이콘 등록
        icon.sprite = effect.Icon;
    }
}