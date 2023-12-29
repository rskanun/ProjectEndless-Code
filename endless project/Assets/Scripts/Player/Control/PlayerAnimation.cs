using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("애니메이터 목록")]
    [SerializeField] private Animator playerAnimator;

    public void SetPlayerAngleAnim(Vector2 angle)
    {
        // 올림 보정
        int x = (angle.x > 0) ? Mathf.CeilToInt(angle.x) : Mathf.FloorToInt(angle.x);
        int y = (angle.y > 0) ? Mathf.CeilToInt(angle.y) : Mathf.FloorToInt(angle.y);

        // 애니메이션 움직임 제어
        playerAnimator.SetInteger("axisH", x);
        playerAnimator.SetInteger("axisV", y);
    }

    public IEnumerator AttackAnim()
    {
        // 애니메이션 실행

        // 애니메이션 딜레이
        float delay = 0.5f;
        yield return new WaitForSeconds(delay);
    }
}