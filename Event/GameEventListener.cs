using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent;

    [SerializeField]
    private UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void EventHandler()
    {
        response?.Invoke();
    }
}