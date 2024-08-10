using UnityEngine;

public class ActionSelectionController : MonoBehaviour, IControlState
{
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