using Assets.Script.Object.Monster;
using Assets.Script.UI.ObjectAnimation.Player;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Player
{
    public class AttackManager : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D colliderComponent;

        [Header("참조 스크립트")]
        [SerializeField] private CharacterAnimation anim;
        [SerializeField] private Object.Player.Player player;

        // 참조 스크립터블 오브젝트
        private PlayerState playerState;

        private void Start ()
        {
            playerState = PlayerState.Instance;
        }

        public void OnAttack(float attackAngle)
        {
            playerState.IsAttacking = true;

            // 공격(마우스) 방향으로 공격 범위 이동
            rotateAttackBox(attackAngle);

            // 공격 실행
            StartCoroutine(attackAction());
        }

        private void rotateAttackBox(float angle)
        {
            // 해당 각도로 공격 범위 회전
            float zAngle = Mathf.Round(angle / 45.0f) * 45.0f + 90;

            transform.rotation = Quaternion.Euler(0f, 0f, zAngle);
        }

        private IEnumerator attackAction()
        {
            colliderComponent.enabled = true;

            // 공격 모션 실행
            yield return StartCoroutine(anim.AttackAnimation());

            colliderComponent.enabled = false;
            playerState.IsAttacking = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(Tag.Monster))
            {
                collision.GetComponent<Monster>().OnTakeDamage(player.AttackDamage, player.MP);
            }
        }
    }
}