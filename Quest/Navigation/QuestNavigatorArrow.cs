using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestNavigatorArrow : MonoBehaviour
{
    [Title("네비게이션 설정")]
    public float moveSpeed;

    private Coroutine moveCoroutine;

    public void StartMove(List<Vector2> path)
    {
        moveCoroutine = StartCoroutine(MoveCoroutine(path));
    }

    public void StopMove()
    {
        StopCoroutine(moveCoroutine);
    }

    private IEnumerator MoveCoroutine(List<Vector2> path)
    {
        foreach (Vector2 target in path)
        {
            // 목표까지 이동
            while ((Vector2)transform.position != target)
            {
                MoveTo(target);
                RotateTo(target);

                yield return null;
            }
        }
    }

    private void MoveTo(Vector2 target)
    {
        float speed = moveSpeed * Time.deltaTime;

        // 현재 위치에서 움직일 위치와 목표까지의 거리 계산
        transform.position = Vector2.MoveTowards(transform.position, target, speed);
    }

    private void RotateTo(Vector2 target)
    {
        var dir = (target - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f;

        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}