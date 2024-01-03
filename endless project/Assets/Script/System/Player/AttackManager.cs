using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Player
{
    public class AttackManager : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D colliderComponent;

        [Header("참조 스크립트")]
        [SerializeField] private PlayerAnimation anim;

        // 참조 스크립터블 오브젝트
        private OldPlayerState playerState;

        // 공격 데미지 및 MP
        private int _damage;
        private int _mp;

        private void Start ()
        {
            playerState = OldPlayerState.Instance;
        }

        public void OnAttack(float attackAngle, int damage, int playerMP)
        {
            playerState.IsAttacking = true;

            // 데미지 설정
            _damage = damage;
            _mp = playerMP;

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
            yield return StartCoroutine(anim.AttackAnim());

            colliderComponent.enabled = false;
            playerState.IsAttacking = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Monster"))
            {
                collision.GetComponent<Monster>().OnTakeDamage(_damage, _mp);
            }
        }
    }
}