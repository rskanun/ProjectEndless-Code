using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class AttackManager : MonoBehaviour
{
    private PolygonCollider2D atkCollider;
    private SpriteRenderer spriteRenderer;

#if UNITY_EDITOR
    private void OnValidate()
    {
        atkCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
#endif

    public void UpdateAttackCollider()
    {
        // 현재 프레임의 공격 범위(콜라이더) 가져오기
        List<Vector2> shapes = new List<Vector2>();
        spriteRenderer.sprite.GetPhysicsShape(0, shapes);

        // 해당 콜라이더를 이에 맞게 변형
        atkCollider.SetPath(0, shapes);
    }
}