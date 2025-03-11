using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("공격 범위")]
    [SerializeField] private AttackArea downArea;
    [SerializeField] private AttackArea leftArea;
    [SerializeField] private AttackArea upArea;
    [SerializeField] private AttackArea rightArea;

    private BoxCollider2D collider;

#if UNITY_EDITOR
    private void OnValidate()
    {
        collider = GetComponent<BoxCollider2D>();
    }
#endif

    public void SetRotate(Vector2 dir)
    {
        여기
    }

    [System.Serializable]
    private class AttackArea
    {
        public Vector2 pos;
        public Vector2 size;
    }
}