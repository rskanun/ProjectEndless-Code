using System.Collections;
using UnityEngine;

namespace Assets.Script.UI.ObjectAnimation.Player
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;

        [Header("플레이어 데이터")]
        [SerializeField]
        private PlayerData player;

        private void Awake()
        {
            player.OnPlayerAngleChanged.AddListener(UpdateAnimationByPlayerSight);
        }

        public void UpdateAnimationByPlayerSight(Vector2 angle)
        {
            // 올림 보정
            int x = (angle.x > 0) ? Mathf.CeilToInt(angle.x) : Mathf.FloorToInt(angle.x);
            int y = (angle.y > 0) ? Mathf.CeilToInt(angle.y) : Mathf.FloorToInt(angle.y);

            // 애니메이션 움직임 제어
            playerAnimator.SetInteger("axisH", x);
            playerAnimator.SetInteger("axisV", y);
        }

        public IEnumerator AttackAnimation()
        {
            // 애니메이션 실행

            // 애니메이션 딜레이
            float delay = 0.5f;
            yield return new WaitForSeconds(delay);
        }
    }
}