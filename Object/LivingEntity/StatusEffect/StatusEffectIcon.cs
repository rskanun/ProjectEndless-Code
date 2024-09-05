using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIcon : MonoBehaviour
{
    public Image icon;
    public CanvasGroup canvasGroup;

    public void SetIcon(Sprite image)
    {
        icon.sprite = image;
    }

    public void SetAlpha(float a)
    {
        canvasGroup.alpha = a;
    }
}