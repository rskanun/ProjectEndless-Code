using System;
using TMPro;
using UnityEngine;

public class SelectOption : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI optionText;

    private Action onClickHandler;

    public void SetOption(string option)
    {
        optionText.text = option;
    }

    public void SetHandler(Action handler)
    {
        onClickHandler = handler;
    }

    public void OnClick()
    {
        onClickHandler?.Invoke();
    }
}