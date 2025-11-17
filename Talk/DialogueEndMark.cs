using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueEndMark : MonoBehaviour
{
    [Space]
    [Title("애니메이션 설정")]
    [SerializeField] private float moveRange;
    [SerializeField] private float duration;

    private Vector3 originPos;

    private void OnValidate()
    {
        // 본래 위치 기억하기
        originPos = transform.localPosition;
    }

    private void OnEnable()
    {
        // 초기 위치 설정
        transform.localPosition = originPos;

        // 활성화 시 애니메이션 실행
        transform.DOLocalMoveY(transform.localPosition.y + moveRange, duration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        // 실행 중이던 애니메이션 제거
        transform.DOKill();
    }
}