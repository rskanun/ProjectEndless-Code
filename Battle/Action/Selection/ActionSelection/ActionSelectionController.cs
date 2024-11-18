using UnityEngine;

public class ActionSelectionController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection selection;
    [SerializeField] private ActionManager actionManager;

    public void OnConnected()
    {

    }

    public void OnDisconnected()
    {

    }

    public void OnActionSelectKeyPressed()
    {
        // 누른 키에 따른 행동 선택
        // ex) a키 -> 공격, s키 -> 스킬
    }
}