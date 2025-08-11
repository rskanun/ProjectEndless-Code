using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimerUI : AppUI
{
    [Title("타이머")]
    [SerializeField] private RotaryTimer hourTimer;
    [SerializeField] private RotaryTimer minTimer;
    [SerializeField] private TextMeshProUGUI secTimer;
    [Space]
    [SerializeField] private TextMeshProUGUI simpleRemainTimer;
    [SerializeField] private TextMeshProUGUI detailRemainTimer;
    [SerializeField] private AmountBar amountTime;
    [Space]
    [SerializeField] private CanvasGroup simpleInfo;
    [SerializeField] private CanvasGroup detailInfo;
    [SerializeField] private AmountTextBar hpBar;
    [SerializeField] private AmountTextBar spBar;

    private bool isShowDetail;

    // 애니메이션 설정
    private Sequence infoAnimation;
    private float infoDuration = 0.25f;

    protected override void ActiveAppWithAnimation(Action openHandler)
    {
        MenuAnimation.AppOpenAnimation(window, appBackground, openHandler)
            .AppendCallback(() => _isOpened = true);
    }

    protected override void DeactiveAppWithAnimation()
    {
        MenuAnimation.AppCloseAnimation(window, appBackground);
    }

    public void SelectFirstTimer()
    {
        EventSystem.current.SetSelectedGameObject(hourTimer.gameObject);
    }

    public void SetTime(Endless.GameData.Time time)
    {
        hourTimer.SetTime(time.Hour);
        minTimer.SetTime(time.Min);

        // 초는 사용 X
        secTimer.text = $"{time.Sec:d2}";
    }

    public Endless.GameData.Time GetTime()
    {
        int hour = hourTimer.currentTime;
        int min = minTimer.currentTime;

        return new Endless.GameData.Time(hour, min, 0);
    }

    public void SetRemainTime(Endless.GameData.Time time)
    {
        Endless.GameData.Time maxTime = GameData.Instance.MaxTime;

        string text = $"<b>{time}</b> <size=12>남음</size>";

        simpleRemainTimer.text = text;
        detailRemainTimer.text = text;
        amountTime.UpdateAmount(time.TotalSeconds, maxTime.TotalSeconds);
    }

    public void SetTimeRange(Endless.GameData.Time time)
    {
        hourTimer.SetMaxTime(time.Hour + 1); // 해당 시간도 포함해야 함
        minTimer.SetMaxTime((hourTimer.currentTime == time.Hour) ? time.Min : 60);
    }

    public void ShowRegenAmount()
    {
        // 이미 디테일 창이 띄워져 있다면 무시
        if (isShowDetail) return;

        isShowDetail = true;
        detailInfo.alpha = 0.0f;
        detailInfo.gameObject.SetActive(true);

        // 현재 진행 중인 애니메이션 있다면 종료
        infoAnimation?.Kill();

        // 애니메이션 실행
        infoAnimation = DOTween.Sequence()
            .Join(simpleInfo.DOFade(0.0f, infoDuration))
            .Join(detailInfo.DOFade(1.0f, infoDuration))
            .OnKill(() =>
            {
                simpleInfo.alpha = 1.0f;
                simpleInfo.gameObject.SetActive(false);
            });
    }

    public void HideRegenAmount()
    {
        isShowDetail = false;
        simpleInfo.alpha = 0.0f;
        simpleInfo.gameObject.SetActive(true);

        // 현재 진행 중인 애니메이션 있다면 종료
        infoAnimation?.Kill();

        // 애니메이션 실행
        infoAnimation = DOTween.Sequence()
            .Join(simpleInfo.DOFade(1.0f, infoDuration))
            .Join(detailInfo.DOFade(0.0f, infoDuration))
            .OnKill(() =>
            {
                detailInfo.alpha = 1.0f;
                detailInfo.gameObject.SetActive(false);
            });
    }

    public void SetRegenAmount(int hpPercent, int spPercent)
    {
        hpBar.UpdateAmount((int)MathF.Min(hpPercent, 100), 100);
        spBar.UpdateAmount((int)MathF.Min(spPercent, 100), 100);
    }
}