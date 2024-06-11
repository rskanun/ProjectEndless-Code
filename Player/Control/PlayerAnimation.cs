using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("애니메이터 목록")]
    [SerializeField] private Animator playerAnimator;

    public void SetPlayerAngleAnim(Vector2 angle)
    {
        int x = (int)angle.x;
        int y = (int)angle.y;

        bool isChangedX = false;
        bool isChangedY = false;

        // 애니메이션 움직임 제어
        if (playerAnimator.GetInteger("axisH") != x)
        {
            isChangedX = true;
            playerAnimator.SetInteger("axisH", x);
        }
        if (playerAnimator.GetInteger("axisV") != y)
        {
            isChangedY = true;
            playerAnimator.SetInteger("axisV", y);
        }

        playerAnimator.SetBool("isChanged", isChangedX || isChangedY);
    }
}