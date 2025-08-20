using UnityEngine;

public class OptionApp : SubWindowApp
{
    [SerializeField]
    private OptionUI optionUI;

    protected override void OnOpen()
    {
        // Load Option Data
    }

    protected override void OnClose()
    {
        // Save Option Data

        // if not changed
        // -> exit

        // else
        // check this option saved
    }

    public override void OpenSubWindow(GameObject subWindow)
    {
        base.OpenSubWindow(subWindow);

        // 서브 창을 오픈할 때, 취소 패널도 같이 활성화
        optionUI.SetCancelPanel(true);
    }

    public override void Close()
    {
        base.Close();

        // 모든 서브 창을 다 닫았다면, 취소 패널도 같이 비활성화
        if (subWindows.Count <= 0)
            optionUI.SetCancelPanel(false);
    }
}