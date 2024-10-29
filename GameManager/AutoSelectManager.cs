using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelector
{
    private static GameObject _selectedObj;
    public static GameObject SelectedObj
    {
        private set { _selectedObj = value; }
        get { return _selectedObj; }
    }

    public static void SetSelectedObject(GameObject selectedObj)
    {
        SelectedObj = selectedObj;

        EventSystem.current.SetSelectedGameObject(selectedObj);
    }
}

public class AutoSelectManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject firstSelected = EventSystem.current.firstSelectedGameObject;

        AutoSelector.SetSelectedObject(firstSelected);
    }

    private void Update()
    {
        UpdateLastSelected();
        DiselectedHandler();
    }

    private void UpdateLastSelected()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (AutoSelector.SelectedObj != currentSelected && currentSelected != null)
        {
            // 현재 선택된 것과 값이 다르면 갱신
            AutoSelector.SetSelectedObject(currentSelected);
        }
    }

    private void DiselectedHandler()
    {
        if (AutoSelector.SelectedObj != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(AutoSelector.SelectedObj);
        }
    }
}