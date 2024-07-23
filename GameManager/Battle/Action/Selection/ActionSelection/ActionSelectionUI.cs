using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSelectionUI : MonoBehaviour
{
    [Header("행동 선택창")]
    [SerializeField] private GameObject actionWindow;

    [Header("행동 선택 버튼")]
    [SerializeField] private List<Button> actionButtons;

    private Button lastSelectedButton;

    public void OpenSelectionWindow()
    {
        actionWindow.SetActive(true);

        // 마지막으로 사용한 버튼 활성화
        SelectLastSelctedButton();
    }

    public void CloseSelectionWindow()
    {
        actionWindow.SetActive(false);

        // 버튼 비활성화
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void SelectLastSelctedButton()
    {
        if (lastSelectedButton == null)
            lastSelectedButton = actionButtons[0];

        lastSelectedButton.Select();
    }

    private void Update()
    {
        if (actionWindow.activeSelf)
        {
            GameObject selectButtonObj = lastSelectedButton?.gameObject;

            EventSystem.current.SetSelectedGameObject(selectButtonObj);
        }
    }
}