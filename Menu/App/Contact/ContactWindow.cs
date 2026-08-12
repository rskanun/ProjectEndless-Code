using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContactWindow : MonoBehaviour
{
    [SerializeField] protected RectTransform content;
    [SerializeField] protected VerticalLayoutGroup layoutGroup;
    [SerializeField] protected RectTransform viewportRect;
    [SerializeField] protected CanvasGroup darkPanel;
    protected List<GameObject> contactList = new();

    protected bool _isTweening;
    public bool IsTweening => _isTweening;

    // 애니메이션 설정
    protected HashSet<Tween> playAnimations = new();
    protected float offsetY = 200f;  // 오픈 애니메이션이 시작되는 Y 위치
    protected float offsetX = 300f;   // 클로즈 애니메이션이 끝나는 X 위치
    protected float interval = 0.05f; // 각 항목 등장 간격
    protected float duration = 0.3f; // 올라오는데 걸리는 시간
    private float hideShowMoveRange = 80.0f; // 숨김보임 애니메이션 좌우 이동거리
    private float hideShowDuration = 0.15f; // // 숨김보임 애니메이션 걸리는 시간

    public void KillAnimations()
    {
        foreach (Tween tween in playAnimations)
        {
            tween.Kill();
        }
    }

    public void OpenWindow()
    {
        layoutGroup.enabled = true;

        InitContact();
        StartCoroutine(OpenAnimation());
    }

    public void CloseWindow()
    {
        StartCoroutine(CloseAnimation());
    }

    /// <summary>
    /// 스킬 정보를 위해 잠시 숨겨놓았던 화면 불러오기
    /// </summary>
    public void ShowWindow()
    {
        _isTweening = true;

        // 본래 위치 저장
        float originPosX = content.localPosition.x;

        // 화면 위치 옮겨놓기
        content.transform.localPosition -= new Vector3(hideShowMoveRange, 0);

        // 화면 복구 애니메이션
        DOTween.Sequence()
            .Join(content.transform.DOLocalMoveX(originPosX, hideShowDuration))
            .AppendCallback(() => _isTweening = false);
    }

    /// <summary>
    /// 스킬 정보를 위해 화면 잠시 숨기기
    /// </summary>
    public void HideWindow()
    {
        _isTweening = true;

        float originPosX = content.localPosition.x;

        // 화면 숨김 애니메이션 실행
        DOTween.Sequence()
            .Join(content.transform.DOLocalMoveX(originPosX - hideShowMoveRange, hideShowDuration)) // 화면을 통째로 옆으로 이동
            .Join(darkPanel.DOFade(0.5f, hideShowDuration)) // 화면 점점 어둡게
            .OnKill(() =>
            {
                content.transform.localPosition = new Vector3(originPosX, content.transform.localPosition.y);
                darkPanel.alpha = 0.0f;
                _isTweening = false;
            });
    }

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
            Tween tween = content.DOLocalMoveY(endValue, 0.2f);
            tween.OnKill(() => playAnimations.Remove(tween));

            playAnimations.Add(tween);
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
            Tween tween = item.DOAnchorPos(targetPos, duration)
                .SetDelay(interval * count++)
                .SetEase(Ease.OutCubic);
            tween.OnKill(() => playAnimations.Remove(tween));

            playAnimations.Add(tween);
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(count * interval + duration);

        // 애니메이션 종료 후 레이아웃 그룹 다시 작동
        layoutGroup.enabled = true;

        // 코루틴 애니메이션 종료 선언
        _isTweening = false;
    }

    protected virtual IEnumerator CloseAnimation()
    {
        _isTweening = true;

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
            Tween tween = item.DOAnchorPos(targetPos, duration)
                .SetDelay(count++ * interval)
                .SetEase(Ease.OutCubic);
            tween.OnKill(() => playAnimations.Remove(tween));

            playAnimations.Add(tween);
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

    protected void DestroyContactObjs()
    {
        foreach (GameObject obj in contactList)
        {
            Destroy(obj);
        }
    }

    protected abstract void InitContact();
}