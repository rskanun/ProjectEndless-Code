using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectedData
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

        AutoSelectedData.SetSelectedObject(firstSelected);
    }

    private void Update()
    {
        if (AutoSelectedData.SelectedObj != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(AutoSelectedData.SelectedObj);
        }
    }
}