using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("애니메이터 목록")]
    [SerializeField] private Animator playerAnimator;

    public void SetPlayerAngleAnim(Vector2 angle)
    {
        int h = (int)angle.x;
        int v = (int)angle.y;

        int curH = playerAnimator.GetInteger("axisH");
        int curV = playerAnimator.GetInteger("axisV");

        if (curV == 0 && curH != h)
        {
            playerAnimator.SetBool("isChanged", true);
            playerAnimator.SetInteger("axisH", h);
        }
        else if (curH == 0 && curV != v)
        {
            playerAnimator.SetBool("isChanged", true);
            playerAnimator.SetInteger("axisV", v);
        }
        else
        {
            playerAnimator.SetBool("isChanged", false);
        }
    }
}