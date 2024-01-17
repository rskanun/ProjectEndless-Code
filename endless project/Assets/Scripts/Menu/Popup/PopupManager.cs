using System.Collections.Generic;

public class PopupManager
{
    private static PopupManager _instance;
    public static PopupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PopupManager();
            }

            return _instance;
        }
    }

    private List<Popup> popupList = new List<Popup>();

    public bool isActive
    {
        get { return popupList.Count > 0; }
    }

    public void Add(Popup popup)
    {
        popupList.Add(popup);
    }

    public void Destroy(Popup popup)
    {
        popupList.Remove(popup);
        popup.Destroy();
    }

    public void Close()
    {
        if (popupList.Count > 0)
        {
            int index = popupList.Count - 1;
            popupList[index].Close();
        }
    }
}