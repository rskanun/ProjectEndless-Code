using UnityEngine;

public abstract class App : MonoBehaviour
{
    [Header("실행할 앱 창")]
    [SerializeField] private GameObject window;

    // 앱 현재상황
    private bool _isActive;
    public bool IsActive
    {
        private set { _isActive = value; }
        get { return _isActive; }
    }

    [Header("참조 스크립트")]
    [SerializeField] private AppUI ui;

    public virtual void Open()
    {
        IsActive = true;

        ui.OpenApp(window);
        LoadData();
    }

    protected virtual void LoadData() { }

    public virtual void Close()
    {
        SaveData();
        ui.CloseApp(window);

        IsActive = false;
    }

    protected virtual void SaveData() { }
}