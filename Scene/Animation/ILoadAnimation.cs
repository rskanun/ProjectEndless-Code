using Cysharp.Threading.Tasks;

public interface ILoadAnimation
{
    public UniTask PlayAnimation();
    public void StopAnimation();
}