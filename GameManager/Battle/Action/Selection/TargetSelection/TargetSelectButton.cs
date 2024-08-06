using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TargetSelectButton : MonoBehaviour
{
    public bool interactable;
    public Image targetGraphic;
    public Sprite selectedSprite;

    private bool isSelected;

    [SerializeField]
    private UnityEvent onClick;

    public void OnHover()
    {
        if (interactable)
        {
            isSelected = !isSelected;

            SetSelected(isSelected);
        }
    }

    public void OnClick()
    {
        if (interactable)
        {
            onClick?.Invoke();
        }
    }

    private void SetSelected(bool isSelected)
    {
        if (isSelected) targetGraphic.sprite = selectedSprite;
        else targetGraphic.sprite = null;
    }
}