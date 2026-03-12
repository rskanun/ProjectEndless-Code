using System.Collections;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestNavigation : MonoBehaviour
{
    [SerializeField]
    private QuestNavigatorArrow navigatorArrow;
    private SpriteRenderer naviRenderer;

    [Title("네비게이션 설정")]
    [SerializeField]
    private float reachDistance;
    [SerializeField, MinValue(0.5f)]
    private float duration;
    [SerializeField, MinValue(0.5f)]
    private float delay;

    private Coroutine animCoroutine;
    private Coroutine arriveCheckCoroutine;

    public Vector2 testTarget;

    private void OnValidate()
    {
        naviRenderer = navigatorArrow?.GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(testTarget, 0.5f);
    }

    [Button("Navigation Test", ButtonSizes.Large)]
    public void Test()
    {
        StartNavigation(testTarget);
    }

    public void StartNavigation(Vector2 target)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        if (arriveCheckCoroutine != null) StopCoroutine(arriveCheckCoroutine);

        // 애니메이션 코루틴
        animCoroutine = StartCoroutine(DrawRoute(target));

        // 도달 확인 코루틴
        arriveCheckCoroutine = StartCoroutine(CheckArrived(target));
    }

    private IEnumerator DrawRoute(Vector2 target)
    {
        var gameData = GameData.Instance;
        var originColor = naviRenderer.color;

        // 방향 오브젝트 켜기
        navigatorArrow.gameObject.SetActive(true);

        // 애니메이션 계속 진행
        // 종료는 다른 코루틴에서 체크하고 진행
        while (true)
        {
            // 현재 플레이어 위치에서 목표까지의 경로 탐색
            var path = Navigator.FindPath(gameData.MapGrid, gameData.Position, target);

            // 네비 설정
            naviRenderer.color = originColor;
            navigatorArrow.transform.position = path.FirstOrDefault();

            // 해당 경로를 따라 화살표 오브젝트 움직이기
            navigatorArrow.StartMove(path);

            // 점점 사라지는 애니메이션 실행
            naviRenderer.DOFade(0.0f, duration);

            // 애니메이션 + 딜레이까지 대기
            yield return new WaitForSeconds(duration + delay);

            // 완전히 사라진 경우 화살표 이동 멈추기
            navigatorArrow.StopMove();
        }
    }

    private IEnumerator CheckArrived(Vector2 target)
    {
        var gameData = GameData.Instance;
        bool isArrived()
            => Vector2.Distance(gameData.Position, target) < reachDistance;

        // 도착할 때까지 대기
        yield return new WaitUntil(isArrived);

        // 진행 중이던 애니메이션 종료
        StopCoroutine(animCoroutine);

        // 진행 중이던 DOTween 애니메이션 종료
        naviRenderer.DOKill();

        // 방향 오브젝트 초기화 및 비활성화
        var color = naviRenderer.color;
        color.a = 1.0f;
        naviRenderer.color = color;
        navigatorArrow.gameObject.SetActive(false);
    }
}