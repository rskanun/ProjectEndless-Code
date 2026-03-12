public abstract class Popup
{
    public Popup()
    {
        PopupManager.Instance.Add(this);
    }

    public abstract void Show();
    public abstract void Destroy();
    public abstract void Close();
}