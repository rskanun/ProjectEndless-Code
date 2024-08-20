using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject actionWindow;
    [SerializeField] private Button firstSelectedButton;

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
            lastSelectedButton = firstSelectedButton;

        SelectionData.SetSelectedObject(lastSelectedButton.gameObject);
    }

    public void SetLastSelectedButton(Button button)
    {
        lastSelectedButton = button;
    }
}