using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private TextMeshProUGUI contents;
    [Space]
    [Header("확인 버튼")]
    [SerializeField] private TextMeshProUGUI yesTxt;
    [Space]
    [Header("취소 버튼")]
    [SerializeField] private TextMeshProUGUI noTxt;

    public delegate void PopupCallBack();
    private event PopupCallBack yesCallBack;
    private event PopupCallBack noCallBack;

    public void SetConfirm(string msg, string yesText, string noText)
    {
        contents.text = msg;
        yesTxt.text = yesText;
        noTxt.text = noText;
    }

    public void SetYesCallBack(PopupCallBack listener)
    {
        yesCallBack = listener;
    }

    public void SetNoCallBack(PopupCallBack listener)
    {
        noCallBack = listener;
    }

    public void OnClickYes()
    {
        yesCallBack?.Invoke();
    }

    public void OnClickNo()
    {
        noCallBack?.Invoke();
    }

    public Sequence Show()
    {
        darkPanel.SetActive(true);

        return MenuAnimation.PopupOpenAnimation(popup);
    }

    public Sequence Hide()
    {
        darkPanel.SetActive(false);

        return MenuAnimation.PopupCloseAnimation(popup);
    }
}