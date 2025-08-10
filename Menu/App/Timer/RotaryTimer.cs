using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RotaryTimer : MonoBehaviour, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler
{
    [SerializeField] private TextMeshProUGUI mainTime;
    [SerializeField] private TextMeshProUGUI ceilTime;
    [SerializeField] private TextMeshProUGUI overCeilTime;
    [SerializeField] private TextMeshProUGUI floorTime;
    [SerializeField] private TextMeshProUGUI overFloorTime;
    [Space]
    [SerializeField] private int maxTime;
    [SerializeField] private int interval;
    [Space]

    // 인스펙터창에서 가장 나중에 표시
    [SerializeField, PropertyOrder(100)]
    private UnityEvent onChanged;
    [SerializeField, PropertyOrder(100)]
    private UnityEvent onSubmit;

    // 애니메이션 변수
    private Sequence selectAnimation;
    private Sequence rollAnimation;
    private int timerDistance = 30;
    private float duration = 0.45f;
    private float fadeAlpha = 0.25f;
    private float rollDuration = 0.3f;
    private Ease moveEase = Ease.OutQuart;
    public bool isRolled { get; private set; }

    [ShowInInspector]
    public int currentTime { get; private set; }

    private void OnValidate()
    {
        if (mainTime == null || ceilTime == null || overCeilTime == null || floorTime == null || overFloorTime == null)
        {
            return;
        }

        SetTime(currentTime);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 타이머 본래 위치 기억하기
        Vector3 ceilPos = ceilTime.transform.localPosition;
        Vector3 floorPos = floorTime.transform.localPosition;

        // 메인 타이머 위치 이동
        ceilTime.transform.position = mainTime.transform.position;
        floorTime.transform.position = mainTime.transform.position;

        // 위아래로 타이머 이동 애니메이션
        Sequence moveAnimation = DOTween.Sequence()
            .Join(ceilTime.transform.DOLocalMoveY(ceilPos.y, duration))
            .Join(floorTime.transform.DOLocalMoveY(floorPos.y, duration))
            .SetEase(moveEase);

        // 타이머 알파값 설정
        ceilTime.alpha = 0.0f;
        floorTime.alpha = 0.0f;

        // 타이머 페이드 애니메이션
        Sequence fadeAnimation = DOTween.Sequence()
            .Join(ceilTime.DOFade(fadeAlpha, duration))
            .Join(floorTime.DOFade(fadeAlpha, duration));

        // 애니메이션 실행
        selectAnimation = DOTween.Sequence()
            .Join(moveAnimation)
            .Join(fadeAnimation)
            .OnKill(() =>
            {
                ceilTime.transform.localPosition = ceilPos;
                floorTime.transform.localPosition = floorPos;
            });
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // 실행 중인 애니메이션이 있다면 중지
        selectAnimation?.Kill();
        rollAnimation?.Kill();

        // 타이머 알파값을 조정하여 숨기기
        ceilTime.alpha = 0.0f;
        floorTime.alpha = 0.0f;
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Up) RollTimer(false);
        else if (eventData.moveDir == MoveDirection.Down) RollTimer(true);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onSubmit?.Invoke();
    }

    public void SetMaxTime(int maxTime)
    {
        this.maxTime = maxTime;

        // 최대 시간 변화에 따른 현재 시간 변경
        SetTime(currentTime);
    }

    public void SetTime(int time)
    {
        time = Mathf.Clamp(time, 0, maxTime);

        overCeilTime.text = $"{ClampTime(time - 2 * interval):d2}";
        ceilTime.text = $"{ClampTime(time - 1 * interval):d2}";
        mainTime.text = $"{time:d2}";
        floorTime.text = $"{ClampTime(time + 1 * interval):d2}";
        overFloorTime.text = $"{ClampTime(time + 2 * interval):d2}";

        currentTime = time;
    }

    private int ClampTime(int time)
    {
        // 시간에 맞게 순환
        if (time < 0)
            return (int)MathF.Max(maxTime + time, 0);
        if (time >= maxTime)
            return (int)MathF.Min(time - maxTime, Mathf.Max(maxTime - interval, 0));
        return time;
    }

    public void RollTimer(bool isUpper)
    {
        // 이미 애니메이션이 작동 중이면 무시
        if (isRolled) return;

        isRolled = true;
        var origin = mainTime.transform.localPosition;
        var animTime = isUpper ? floorTime : ceilTime;

        // 애니메이션 실행
        float endPos = isUpper ? timerDistance : -timerDistance;
        rollAnimation = DOTween.Sequence()
            .Append(mainTime.transform.DOLocalMoveY(endPos, rollDuration)) // 시계 돌아가는 애니메이션
            .Join(mainTime.DOFade(fadeAlpha, rollDuration)) // 본래 시계의 알파값 낮추기
            .Join(animTime.DOFade(1.0f, rollDuration)) // 애니메이션용 시계는 알파값 높이기
            .OnKill(() =>
            {
                int direction = isUpper ? 1 : -1;

                // 시계의 문자 바꾸기
                if (maxTime > 0)
                    SetTime((currentTime + maxTime + interval * direction) % maxTime);

                mainTime.transform.localPosition = origin; // 본래 위치로 돌리기
                mainTime.alpha = 1.0f; // 본래 시계의 글자의 알파값 되돌리기
                animTime.alpha = fadeAlpha; // 애니메이션에 쓰인 글자도 되돌리기

                // 업데이트 알림
                onChanged?.Invoke();

                // 애니메이션 종료
                isRolled = false;
            });
    }
}