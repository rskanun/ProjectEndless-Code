using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContactWindow : MonoBehaviour
{
    [SerializeField] protected RectTransform content;
    [SerializeField] protected VerticalLayoutGroup layoutGroup;
    [SerializeField] protected RectTransform viewportRect;

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

    /// <summary>
    /// 선택된 연락처 오브젝트 위치에 맞춰 전체적인 스크롤뷰 이동
    /// </summary>
    /// <param name="focusContact">선택된 연락처</param>
    protected virtual void UpdateScrollPosition(GameObject focusContact)
    {
        // 화면에 보여지는 최소 최대 y값
        float minY = -content.localPosition.y - viewportRect.rect.height + layoutGroup.padding.bottom;
        float maxY = -content.localPosition.y - layoutGroup.padding.top;

        // 해당 연락처 오브젝트의 최하단 및 최상단 y값
        RectTransform rectTransform = focusContact.GetComponent<RectTransform>();
        float contactMinY = focusContact.transform.localPosition.y - rectTransform.rect.height;
        float contactMaxY = focusContact.transform.localPosition.y;

        // 화면에 오브젝트가 일부라도 잘리는 지 판단
        float endValue = content.localPosition.y;
        if (contactMinY < minY)
        {
            // 하단이 잘렸다면 잘려나가는 부분만큼 스크롤을 위로 올리기
            endValue = content.localPosition.y + minY - contactMinY;
        }
        else if (contactMaxY > maxY)
        {
            // 상단이 잘렸다면 잘려나가는 부분만큼 스크롤을 아래로 내리기
            endValue = content.localPosition.y - contactMaxY + maxY;
        }

        // 잘려나간 부분이 나오도록 스크롤 조정
        if (endValue != content.localPosition.y)
        {
            content.DOLocalMoveY(endValue, 0.2f);
        }
    }

    protected virtual IEnumerator OpenAnimation()
    {
        _isTweening = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        // 레이아웃 그룹 잠시 끄기
        layoutGroup.enabled = false;

        // 화면에 보이는 경계 y값
        float maxY = -viewportRect.rect.height;

        int count = 0; // 애니메이션이 실행되는 오브젝트 개수
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) is not RectTransform item) continue;

            // 경계 이하의 오브젝트는 애니메이션 적용 X
            if (item.localPosition.y <= maxY) continue;

            // 현재 위치 저장
            Vector2 targetPos = item.anchoredPosition;

            // 해당 오브젝트를 아래로 내리기
            item.anchoredPosition = targetPos - new Vector2(0, offsetY);

            // 위로 올라오는 애니메이션 실행
            item.DOAnchorPos(targetPos, duration)
                .SetDelay(count++ * interval)
                .SetEase(Ease.OutCubic);
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(count * interval + duration);

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

        // 화면에 표시되는 y 경계값 찾기
        float minY = -content.localPosition.y;
        float maxY = -content.localPosition.y - viewportRect.rect.height;

        int count = 0; // 애니메이션이 실행되는 오브젝트 개수
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) is not RectTransform item) continue;

            float contactMinY = item.localPosition.y;
            float contactMaxY = item.localPosition.y - item.rect.height;

            // 경계값 밖의 오브젝트는 애니메이션 적용 X
            if (minY <= contactMaxY || maxY >= contactMinY) continue;

            Vector2 targetPos = item.anchoredPosition - new Vector2(offsetX, 0);

            // 왼쪽으로 빠지며 페이드 아웃되는 애니메이션 실행
            item.DOAnchorPos(targetPos, duration)
                .SetDelay(count++ * interval)
                .SetEase(Ease.OutCubic);
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(count * interval + duration);

        // 애니메이션 종료 후 레이아웃 그룹 다시 작동
        layoutGroup.enabled = true;

        // 코루틴 애니메이션 종료 선언
        _isTweening = false;

        // 해당 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}