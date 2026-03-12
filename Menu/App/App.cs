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

    public virtual void Open()
    {
        IsActive = true;

        // 앱이 열리는 동안 키 입력 무시
        ControlContext.Instance.KeyLock();

        // 앱 열기 애니메이션
        ui.OpenApp(() =>
        {
            // 키 잠금 해제
            ControlContext.Instance.KeyUnlock();

            // 추가적인 핸들러 실행
            OnOpen();
        });
    }

    public virtual void Close()
    {
        IsActive = false;

        // 앱이 닫히는 동안 키 입력 무시
        ControlContext.Instance.KeyLock();

        OnClose();
        ui.CloseApp(() => ControlContext.Instance.KeyUnlock());
    }

    /// <summary>
    /// 앱 내의 모든 프로세스를 종료하고 닫기
    /// </summary>
    public virtual void Shutdown()
    {
        IsActive = false;

        OnClose();
        ui.DeactiveApp();
    }

    protected virtual void OnOpen() { }

    protected virtual void OnClose() { }
}