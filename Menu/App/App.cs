using System;
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

        ui.OpenApp(isPlayAnimation);
        OnOpened();
    }

    protected virtual void OnOpened() { }

    public virtual void Close(bool isPlayAnimation)
    {
        OnClosed();
        ui.CloseApp(isPlayAnimation);

        IsActive = false;
    }

    protected virtual void OnClosed() { }
}