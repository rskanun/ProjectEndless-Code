using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionData
{
    private static GameObject _selectedObj;
    public static GameObject SelectedObj
    {
        get { return _selectedObj; }
    }

    public static void SetSelectedObject(GameObject selectedObj)
    {
        _selectedObj = selectedObj;

        EventSystem.current.SetSelectedGameObject(selectedObj);
    }
}

public class AutoSelector : MonoBehaviour
{
    private void Awake()
    {
        GameObject firstSelected = EventSystem.current.firstSelectedGameObject;

        SelectionData.SetSelectedObject(firstSelected);
    }

    private void Update()
    {
        if (SelectionData.SelectedObj != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(SelectionData.SelectedObj);
        }
    }
}