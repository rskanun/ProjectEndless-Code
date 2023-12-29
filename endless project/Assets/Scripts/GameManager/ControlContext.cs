using UnityEngine;

public class ControlContext : MonoBehaviour
{
    [Header("컨트롤러 상태")]
    [SerializeField] private PlayerController playerControlState;
    [SerializeField] private IControlState menuControlState;
    [SerializeField] private TalkController talkControlState;

    private IControlState state;

    private void Awake()
    {
        state = playerControlState;
    }

    private void Update()
    {
        state.OnControlKeyPressed(this);
    }

    public void setTalkState(Npc npc)
    {
        state = talkControlState;

        talkControlState.StartTalk(npc);
    }

    public void setMenuState()
    {
        state = menuControlState;
    }

    public void setPlayerState()
    {
        state = playerControlState;
    }
}