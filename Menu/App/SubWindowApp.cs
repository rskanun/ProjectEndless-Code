using System.Collections.Generic;
using UnityEngine;

public abstract class SubWindowApp : App
{
    protected Stack<GameObject> subWindows = new Stack<GameObject>();

    public virtual void OpenSubWindow(GameObject subWindow)
    {
        subWindow.SetActive(true);
        subWindows.Push(subWindow);
    }

    public override void Close(bool isPlayAnimation)
    {
        if (subWindows.Count > 0)
        {
            GameObject subWindow = subWindows.Pop();
            subWindow.SetActive(false);
        }
        else
        {
            // 모든 서브창이 닫혔을 경우 앱 종료
            base.Close(isPlayAnimation);
        }
    }
}