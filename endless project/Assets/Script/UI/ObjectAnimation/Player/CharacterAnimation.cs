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
            int x = Mathf.CeilToInt(angle.x);
            int y = Mathf.CeilToInt(angle.y);

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