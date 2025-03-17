using DG.Tweening;
using UnityEngine;

public class HomeScreenUI : MonoBehaviour
{
    [Header("홈 스크린")]
    [SerializeField] private GameObject homeScreen;

    [Header("앱 버튼")]
    [SerializeField] private GameObject optionButton;
    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject loadButton;
    [SerializeField] private GameObject titleButton;
    [SerializeField] private GameObject callButton;
    [SerializeField] private GameObject messageButton;

    public Sequence DisabledHomeScreen(bool isPlayAnimation)
    {
        SetAllAppButton(false);

        if (isPlayAnimation) return MenuAnimation.HomeScreenHideAnimation(homeScreen);
        else return DOTween.Sequence();
    }

    public Sequence EnabledHomeScreen(bool isPlayAnimation)
    {
        SetAllAppButton(true);

        if (isPlayAnimation) return MenuAnimation.HomeScreenShowAnimation(homeScreen);
        else return DOTween.Sequence();
    }

    public void SetAllAppButton(bool isActive)
    {
        optionButton.SetActive(isActive);
        saveButton.SetActive(isActive);
        loadButton.SetActive(isActive);
        titleButton.SetActive(isActive);
        callButton.SetActive(isActive);
        messageButton.SetActive(isActive);
    }
}