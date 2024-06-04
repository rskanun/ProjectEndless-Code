using System.Collections.Generic;
using UnityEngine;

public abstract class SubWindowApp : App
{
    [SerializeField]
    private SubWindowUI subWindowUI;

    protected Stack<GameObject> subWindows = new Stack<GameObject>();

    public virtual void OpenSubWindow(GameObject subWindow)
    {
        subWindow.SetActive(true);
        subWindows.Push(subWindow);

        subWindowUI.setCancelPanel(true);
    }

    public override void Close()
    {
        if (subWindows.Count > 0)
        {
            GameObject subWindow = subWindows.Pop();
            subWindow.SetActive(false);

            if (subWindows.Count <= 0)
                subWindowUI.setCancelPanel(false);
        }
        else
        {
            // 모든 서브창이 닫혔을 경우 앱 종료
            base.Close();
        }
    }
}