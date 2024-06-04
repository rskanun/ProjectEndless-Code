using UnityEngine;

public abstract class App : MonoBehaviour
{
    [Header("실행할 앱 창")]
    [SerializeField] private GameObject window;

    // 앱 현재상황
    private bool _isActive;
    public bool isActive { get { return _isActive; } }

    [Header("참조 스크립트")]
    [SerializeField] private AppUI ui;

    public virtual void Open()
    {
        _isActive = true;

        ui.OpenApp(window);
        LoadData();
    }

    protected virtual void LoadData() { }

    public virtual void Close()
    {
        SaveData();
        ui.CloseApp(window);

        _isActive = false;
    }

    protected virtual void SaveData() { }
}