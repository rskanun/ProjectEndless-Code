using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private PlayerAnimation anim;

    [Header("플레이어 스텟")]
    [SerializeField] private PlayerData stat;

    [Header("공격 범위 설정")]
    [SerializeField] private Transform atkPos;
    [SerializeField] private Vector2 boxSize;

    [Header("공격 쿨타임(임시)")]
    [SerializeField] private float atkCoolTime;
    private float curTime = 0;

    private bool isAvailableAtk
    {
        get
        {
            return curTime <= 0;
        }
    }

    // 공격 데미지 및 MP
    private int _damage;
    private int _mp;

    private void FixedUpdate()
    {
        if (curTime > 0)
        {
            curTime -= Time.deltaTime;
        }
    }

    public void OnAttack()
    {
        if (isAvailableAtk)
        {
            curTime = atkCoolTime;

            int dmg = stat.AttackDamage;
            int mp = stat.MP;

            // 마우스가 가리키는 방향으로 공격 범위 변경
            float angle = GetRotate();
            SetAtkBoxTransform(angle);

            // 공격 실행
            Collider2D[] colliders = Physics2D.OverlapBoxAll(atkPos.position, boxSize, angle);
            TakeDamageInArea(colliders, dmg, mp);
            anim.OnAttackAnim();
        }
    }

    private float GetRotate()
    {
        Vector2 locClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 locPlayer = transform.position;

        float angle = Mathf.Atan2(locClick.y - locPlayer.y, locClick.x - locPlayer.x)
            * Mathf.Rad2Deg;
        float boxAngle = Mathf.Round(angle / 45.0f) * 45.0f;

        return boxAngle;
    }

    private void SetAtkBoxTransform(float angle)
    {
        float radian = Mathf.Deg2Rad * angle;

        Vector3 direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
        atkPos.position = transform.position + direction * 5f;
    }

    private void TakeDamageInArea(Collider2D[] collisions, int damage, int mp)
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision.CompareTag("Monster"))
            {
                collision.GetComponent<Monster>().OnTakeDamage(damage, mp);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(atkPos.position, boxSize);
    }
}