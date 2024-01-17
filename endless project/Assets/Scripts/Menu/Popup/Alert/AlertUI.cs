using DG.Tweening;
using TMPro;
using UnityEngine;

public class AlertUI : MonoBehaviour
{
    [Header("구성 오브젝트")]
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private TextMeshProUGUI contents;
    [SerializeField] private TextMeshProUGUI okTxt;

    public delegate void PopupCallBack();
    private event PopupCallBack okCallBack;

    public void SetAlert(string msg, string okText)
    {
        contents.text = msg;
        okTxt.text = okText;
    }

    public void SetOkCallBack(PopupCallBack listener)
    {
        okCallBack = listener;
    }

    public void OnClick()
    {
        okCallBack?.Invoke();
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