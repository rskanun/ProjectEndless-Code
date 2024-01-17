using UnityEngine;

public abstract class SubWindowUI : AppUI
{
    [Header("서브창 취소 패널")]
    [SerializeField] private GameObject cancelPanel;

    public void setCancelPanel(bool isVeiw)
    {
        cancelPanel.SetActive(isVeiw);
    }
}