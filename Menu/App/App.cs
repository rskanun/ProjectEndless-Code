using UnityEngine;

public abstract class App : MonoBehaviour
{
    // 앱 현재상황
    private bool _isActive;
    public bool IsActive
    {
        private set { _isActive = value; }
        get { return _isActive; }
    }

    [Header("참조 스크립트")]
    [SerializeField] protected AppUI ui;

    public virtual void Open(bool isPlayAnimation)
    {
        IsActive = true;

        ui.OpenApp(isPlayAnimation, () => OnOpen());
    }

    protected virtual void OnOpen() { }

    public virtual void Close(bool isPlayAnimation)
    {
        // 화면이 완전히 열린 상태가 아니라면 닫기 금지
        if (!ui.IsOpened) return;

        OnClose();
        ui.CloseApp(isPlayAnimation);

        IsActive = false;
    }

    protected virtual void OnClose() { }
}