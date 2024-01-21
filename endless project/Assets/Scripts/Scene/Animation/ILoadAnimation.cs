public interface ILoadAnimation
{
    public delegate void LoadCallBack();

    public void OnLoadAnimation(LoadCallBack listener);
}