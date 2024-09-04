using UnityEngine;

public class ActionSelectionController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection selection;
    [SerializeField] private ActionManager actionManager;

    public void OnControlKeyPressed()
    {
        OnActionSelectKeyPressed();
    }

    public void OnActionSelectKeyPressed()
    {
        // 누른 키에 따른 행동 선택
        // ex) a키 -> 공격, s키 -> 스킬
    }
}