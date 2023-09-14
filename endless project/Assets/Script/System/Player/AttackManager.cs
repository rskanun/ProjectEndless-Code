using Assets.Script.Object.Monster;
using Assets.Script.UI.ObjectAnimation.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Player
{
    public class AttackManager : MonoBehaviour
    {
        [SerializeField] private GameObject normalAttackBox;

        [Header("참조 스크립트")]
        [SerializeField] private CharacterAnimation anim;
        [SerializeField] private Object.Player.Player player;

        // 참조 스크립터블 오브젝트
        private PlayerState playerState;

        private void Start ()
        {
            playerState = PlayerState.Instance;
        }

        public void OnNormalDamage(Collider2D collision)
        {
            float damage = player.Damage;
            float mp = player.MP;

            collision.GetComponent<Monster>().OnTakeDamage(damage, mp);
        }

        public void OnNormalAttack()
        {
            playerState.IsAttacking = true;

            // 공격(마우스) 방향으로 공격 범위 이동
            rotateAttackBox(normalAttackBox);

            // 공격 실행
            StartCoroutine(normalAttackAction());

            playerState.IsAttacking = false;
        }

        private void rotateAttackBox(GameObject attackBox)
        {
            Vector2 locClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 locBox = attackBox.transform.position;

            float xyAngle = Mathf.Atan2(locClick.y - locBox.y, locClick.x - locBox.x)
                * Mathf.Rad2Deg;
            float zAngle = Mathf.Round(xyAngle / 45.0f) * 45.0f + 90;

            attackBox.transform.rotation = Quaternion.Euler(0f, 0f, zAngle);
        }

        private IEnumerator normalAttackAction()
        {
            normalAttackBox.SetActive(true);

            // 공격 모션 실행
            yield return StartCoroutine(anim.AttackAnimation());

            normalAttackBox.SetActive(false);
        }
    }
}