using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContactWindow : MonoBehaviour
{
    [SerializeField] protected RectTransform content;
    [SerializeField] protected VerticalLayoutGroup layoutGroup;

    protected bool _isTweening;
    public bool IsTweening => _isTweening;

    // 애니메이션 설정
    protected float offsetY = 200f;  // 오픈 애니메이션이 시작되는 Y 위치
    protected float offsetX = 300f;   // 클로즈 애니메이션이 끝나는 X 위치
    protected float interval = 0.05f; // 각 항목 등장 간격
    protected float duration = 0.3f; // 올라오는데 걸리는 시간

    public void OpenWindow()
    {
        InitContact();
        StartCoroutine(OpenAnimation());
    }

    protected abstract void InitContact();

    protected virtual IEnumerator OpenAnimation()
    {
        _isTweening = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        // 레이아웃 그룹 잠시 끄기
        layoutGroup.enabled = false;

        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) is not RectTransform item) continue;

            // 현재 위치 저장
            Vector2 targetPos = item.anchoredPosition;

            // 해당 오브젝트를 아래로 내리기
            item.anchoredPosition = targetPos - new Vector2(0, offsetY);

            // 위로 올라오는 애니메이션 실행
            item.DOAnchorPos(targetPos, duration)
                .SetDelay(i * interval)
                .SetEase(Ease.OutCubic);
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(content.childCount * interval + duration);

        // 애니메이션 종료 후 레이아웃 그룹 다시 작동
        layoutGroup.enabled = true;

        // 코루틴 애니메이션 종료 선언
        _isTweening = false;
    }

    public void CloseWindow()
    {
        StartCoroutine(CloseAnimation());
    }

    protected virtual IEnumerator CloseAnimation()
    {
        _isTweening = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        yield return null;

        // 레이아웃 그룹 잠시 끄기
        layoutGroup.enabled = false;

        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) is not RectTransform item) continue;

            Vector2 targetPos = item.anchoredPosition - new Vector2(offsetX, 0);

            // 왼쪽으로 빠지며 페이드 아웃되는 애니메이션 실행
            item.DOAnchorPos(targetPos, duration)
                .SetDelay(i * interval)
                .SetEase(Ease.OutCubic);
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(content.childCount * interval + duration);

        // 애니메이션 종료 후 레이아웃 그룹 다시 작동
        layoutGroup.enabled = true;

        // 코루틴 애니메이션 종료 선언
        _isTweening = false;

        // 해당 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}