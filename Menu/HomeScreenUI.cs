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
    [SerializeField] private GameObject partyButton;

    public Sequence DisabledHomeScreen()
    {
        SetAllAppButton(false);

        return MenuAnimation.HomeScreenHideAnimation(homeScreen);
    }

    public Sequence EnabledHomeScreen()
    {
        SetAllAppButton(true);

        return MenuAnimation.HomeScreenShowAnimation(homeScreen);
    }

    public void SetHomeScreen(bool isActive)
    {
        homeScreen.SetActive(isActive);
    }

    public void SetAllAppButton(bool isActive)
    {
        optionButton.SetActive(isActive);
        saveButton.SetActive(isActive);
        loadButton.SetActive(isActive);
        titleButton.SetActive(isActive);
        callButton.SetActive(isActive);
        messageButton.SetActive(isActive);
        partyButton.SetActive(isActive);
    }
}